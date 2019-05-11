# vue-dotnet-wc-example-ts

## Project setup

```
npm install
```

---

### Compiles and hot-reloads for development

```
npm run serve
```

- Entry point is main.ts

---

### Compiles and minifies for production as web components

```
npm run build
```

- Entry point is /dist/my-vue.js.
- Change the `my-vue` name in the package.json build script `--name` argument. Changing this will require updating the `vuesettings.json` or `web.config` on the dotnet side.
- Remove the ".template" from the included `env.local.template` file and it will copy your changes into that project after build.

---

### Run your tests

```
npm run test
```

---

### Lints and fixes files

```
npm run lint
```

---

### Run your unit tests

```
npm run test:unit
```

---

### Customize configuration

See [Configuration Reference](https://cli.vuejs.org/config/).

---

This project is a pretty basic vue-cli 3 installation. It has a webpack plugin included in the vue.config.js that copies the dist folder into the neighboring dotnet core example project.
