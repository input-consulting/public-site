/*  */
const buildRoot = 'web';
const siteRoot = 'site';

/* */
const sitePublic = 'server/public';

module.exports = {

    title: 'Input Consulting Stockholm AB',

    /* */
    root: buildRoot,
    public: sitePublic,
    site : siteRoot,

    /* used by build tasks */
    build: {

        static: {
            source: [
                buildRoot + '/static/**/*.*'
            ],
            target: sitePublic
        },

        js: {
            source: [
                './node_modules/jquery/dist/jquery.min.js',
                './node_modules/bootstrap-sass/assets/javascripts/bootstrap.js',
                buildRoot + '/src/js/*.js'
            ],
            target: sitePublic + '/js'
        },

        fonts: {
            source: [
                './node_modules/bootstrap-sass/assets/fonts/bootstrap/*.*'
            ],
            target: sitePublic + '/fonts'
        },

        styles: {
            source: [
                buildRoot + '/src/scss/site.scss'
            ],
            target: sitePublic + '/css'
        }
    }
};
