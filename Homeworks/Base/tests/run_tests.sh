#!/bin/sh

httpyac send --all --insecure --output body tests/read.http
httpyac send --all --insecure --output body tests/create.http
httpyac send --all --insecure --output body tests/delete.http
httpyac send --all --insecure --output body tests/update.http
