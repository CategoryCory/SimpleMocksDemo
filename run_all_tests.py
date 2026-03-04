#!/usr/bin/env python3
"""
Unified test runner for the SimpleMocksDemo workspace.

Usage:
  python3 run_all_tests.py [--no-docker] [--docker-down] [--skip-dotnet] [--skip-cpp] [--timeout SECONDS]

Behavior:
- Starts `tax-service` with `docker compose up -d` (unless `--no-docker`).
- Waits for the service to respond on common FastAPI endpoints.
- Runs .NET tests via `dotnet test` for the project/solution found in `OrderProcessor`.
- Runs C++ tests via `ctest` for builds found in `OrderProcessorCpp/build` or `OrderProcessorCpp/cmake-build-debug`.
- Optionally runs `docker compose down` when finished (`--docker-down`).

"""
from __future__ import annotations

import argparse
import os
import subprocess
import sys
import time
import urllib.request
from typing import List, Optional


def run_command(cmd: List[str], cwd: Optional[str] = None) -> int:
    print(f"\n>>> Running: {' '.join(cmd)} (cwd={cwd or '.'})")
    try:
        proc = subprocess.run(cmd, cwd=cwd, check=False)
        return proc.returncode
    except FileNotFoundError:
        print(f"Command not found: {cmd[0]}")
        return 127


def wait_for_http(urls: List[str], timeout: int = 60, interval: float = 1.0) -> bool:
    deadline = time.time() + timeout
    last_err = None
    while time.time() < deadline:
        for u in urls:
            try:
                with urllib.request.urlopen(u, timeout=2) as resp:
                    if 200 <= resp.getcode() < 500:
                        print(f"Service ready at: {u} (status {resp.getcode()})")
                        return True
            except Exception as e:
                last_err = e
        time.sleep(interval)
    print(f"Timed out waiting for service (last error: {last_err})")
    return False


def find_dotnet_target() -> Optional[str]:
    # Prefer the tests project file, else the solution file
    proj = os.path.join("OrderProcessor", "OrderProcessor.Tests", "OrderProcessor.Tests.csproj")
    if os.path.exists(proj):
        return proj
    sln = os.path.join("OrderProcessor", "OrderProcessorDemo.slnx")
    if os.path.exists(sln):
        return sln
    # fallback: attempt to run `dotnet test` in OrderProcessor root
    if os.path.isdir("OrderProcessor"):
        return "OrderProcessor"
    return None


def find_cpp_build_dir() -> Optional[str]:
    candidates = [
        os.path.join("OrderProcessorCpp", "build"),
        os.path.join("OrderProcessorCpp", "cmake-build-debug"),
    ]
    for c in candidates:
        if os.path.isdir(c):
            return c
    return None


def main(argv: List[str]) -> int:
    ap = argparse.ArgumentParser(description="Run all tests across projects")
    ap.add_argument("--no-docker", action="store_true", help="Do not start tax-service in Docker")
    ap.add_argument("--docker-down", action="store_true", help="Bring docker-compose down when finished")
    ap.add_argument("--skip-dotnet", action="store_true", help="Skip running .NET tests")
    ap.add_argument("--skip-cpp", action="store_true", help="Skip running C++ tests")
    ap.add_argument("--timeout", type=int, default=60, help="Seconds to wait for tax-service readiness")
    args = ap.parse_args(argv)

    failures = []

    # 1) Start tax-service
    if not args.no_docker:
        rc = run_command(["docker", "compose", "up", "-d"], cwd="tax-service")
        if rc != 0:
            print("Failed to start tax-service via docker compose up -d")
            failures.append("docker_up")
        else:
            print("Started tax-service (docker compose up -d)")

        # Wait for readiness
        urls = [
            "http://127.0.0.1:8000/",
            "http://127.0.0.1:8000/health",
            "http://127.0.0.1:8000/ready",
            "http://127.0.0.1:8000/ping",
            "http://localhost:8000/",
            "http://localhost:8000/health",
        ]
        if not wait_for_http(urls, timeout=args.timeout):
            print("tax-service did not become ready within timeout")
            failures.append("tax_service_ready")
    else:
        print("--no-docker specified; assuming tax-service is already running")

    # 2) Run .NET tests
    if not args.skip_dotnet:
        target = find_dotnet_target()
        if target is None:
            print("Could not find .NET project/solution to test under OrderProcessor/")
            failures.append("dotnet_missing")
        else:
            # If target is a directory, run dotnet test there
            if os.path.isdir(target):
                rc = run_command(["dotnet", "test", "--no-build"], cwd=target)
            else:
                rc = run_command(["dotnet", "test", target])
            if rc != 0:
                print("dotnet tests failed")
                failures.append("dotnet_tests")

    # 3) Run C++ tests with ctest
    if not args.skip_cpp:
        build_dir = find_cpp_build_dir()
        if build_dir is None:
            print("Could not find C++ build directory under OrderProcessorCpp/")
            failures.append("cpp_build_missing")
        else:
            # Use ctest in that build dir
            rc = run_command(["ctest", "--output-on-failure", "-V"], cwd=build_dir)
            if rc != 0:
                print("C++ tests (ctest) failed or ctest not found")
                failures.append("cpp_tests")

    # 4) Optionally bring docker down
    if args.docker_down and not args.no_docker:
        rc = run_command(["docker", "compose", "down"], cwd="tax-service")
        if rc != 0:
            print("docker compose down failed")
            failures.append("docker_down")

    if failures:
        print(f"\nCompleted with failures: {failures}")
        return 2
    print("\nAll steps completed successfully")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
