#!/bin/sh
DOCKER_TAG=''

case "$CI_COMMIT_REF_NAME" in
  "master")
    DOCKER_TAG=latest
    ;;
  "develop")
    DOCKER_TAG=develop
    ;;
esac

echo "$CI_REGISTRY_PASSWORD" | docker login -u "$CI_REGISTRY_USER" "$CI_REGISTRY" --password-stdin
docker build -t registry.gitlab.com/ruzanowski/ubiquitous/apigateway:${DOCKER_TAG} ./src/ApiGateway
docker push registry.gitlab.com/ruzanowski/ubiquitous/apigateway:${DOCKER_TAG}