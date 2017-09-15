
# Input Consulting - public-site

## Machine Setup

This app is built on node.js, which has a couple of prerequisites you must install first. If you have previously setup your machine for node.js, you can skip this step.

* Install NodeJS >= 8.x
    * You can [download it here](https://nodejs.org/en/).
* Install a Git Client
    * Here's [a nice GUI client](https://desktop.github.com).
    * Here's [a standard client](https://git-scm.com).

Once you have the prerequisites installed, you need a couple of packages. From the command line, use npm to install them globally:

```
npm install gulp -g
npm install nodemon -g
```

> Note: Always run commands from a Bash prompt. Depending on your environment, you may need to use `sudo` when executing npm global installs.

## Application Setup

Once you've setup your machine (or if it's been previous setup), you simply need to install the dependencies. From within the app folder, execute the following command:

```
npm install
```

## Running The App

To run the app, execute the following command from within the app folder:

```
gulp watch
```