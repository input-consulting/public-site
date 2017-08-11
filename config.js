/*  */
const siteRoot = 'site';

/* */
const sitePublic = 'server/public';

module.exports = {

    title: 'Input Consulting Stockholm AB',

    /* */
    root: siteRoot,
    public: sitePublic,

    /* used by build tasks */
    build: {

        static: {
            source: [
                siteRoot + '/static/**/*.*'
            ],
            target: sitePublic
        },

        js: {
            source: [
                './node_modules/jquery/dist/jquery.min.js',
                './node_modules/bootstrap-sass/assets/javascripts/bootstrap.js',
                siteRoot + '/src/js/*.js'
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
                siteRoot + '/src/scss/site.scss'
            ],
            target: sitePublic + '/css'
        }
    }
};