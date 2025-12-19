#!/usr/bin/env bash
# usage: ./nats/health.sh
nats request health.weather.service hello | jq

