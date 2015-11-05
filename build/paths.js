var path = require('path');

var project = 'Input.Site.Web/'
var appRoot = 'app/';
var siteRoot = 'site/';
var views = siteRoot + '/views';
var staticContent = siteRoot + '/static';

module.exports = {
  root: appRoot,
  scripts : {
  	source : [ 
  		'bower_components/jquery/dist/jquery.js',
  		'bower_components/bootstrap/dist/js/bootstrap.js',
        'bower_components/wow/dist/wow.js',
        siteRoot + 'source/js/**/*.js/'
  	],
  	output : appRoot + project + 'assets/js',
  },

  styles : {
  	source : [
        'bower_components/animate.css/animate.css',
        siteRoot + 'source/less/site.less'
    ],
  	output : appRoot + project + 'assets/css',
  },

  fonts : {
      source : [
          'bower_components/bootstrap/dist/fonts/*.*',
          siteRoot + 'source/fonts/*.*',
      ],
    output : appRoot + project + 'assets/fonts'
  },
  
  views : {
      source : views + '/_layout/**/*.html',
      output : appRoot + project + 'views/_layout', 
  },
  
  content : {
      source : [ views + '/**/*.markdown', views + '/**/*.md' ],
      output : appRoot + project + 'views/', 
  },
  static : {
      source : staticContent + '/**/*.*' ,
      output : appRoot + project + 'static', 
  },

  output: appRoot + project + 'assets',

  source: siteRoot + '**/*.js',
  html: siteRoot + '**/*.html',
  style: 'styles/**/*.css',
 
};
