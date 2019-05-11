const { exec } = require('child_process')
const copyLocation = process.env.COPY_DIST_PATH

module.exports = {
  configureWebpack: {
    plugins: [
      {
        apply(compiler) {
          /* This will automatically copy the dist folder to the location specified in the COPY_DIST_PATH env variable */
          compiler.hooks.done.tap(
            'copy-dist-plugin',
            stats =>
              copyLocation && exec('npx copyfiles dist/**/* ' + copyLocation),
          )
        },
      },
    ],
  },
}
