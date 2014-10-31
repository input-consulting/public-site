#!/usr/bin/env bash

rm -r ./app/Input.Site.Web/static
rm -r ./app/Input.Site.Web/views
rm -r ./app/Input.Site.Web.Console/bin/Debug/static
rm -r ./app/Input.Site.Web.Console/bin/Debug/views

cp -r ./site/static ./app/Input.Site.Web/static
cp -r ./site/views ./app/Input.Site.Web/views
cp -r ./site/static ./app/Input.Site.Web.Console/bin/Debug/static
cp -r ./site/views ./app/Input.Site.Web.Console/bin/Debug/views