'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');
var gutil = require('gulp-util');

var repo_root = __dirname + '/';
var govuk_frontend_toolkit_root = repo_root + 'node_modules/govuk_frontend_toolkit/stylesheets'; // 1.
var govuk_elements_sass_root = repo_root + 'node_modules/govuk-elements-sass/public/sass';       // 2.

var buildDir = './build/';

var outputPaths = [
    '/../SFA.Apprenticeships.Web.Recruit/_assets/',
    '/../SFA.Apprenticeships.Web.Manage/_assets/',
    '/../SFA.Apprenticeships.Web.Candidate/_assets/'
]

gutil.log(govuk_frontend_toolkit_root);

// Compile scss files to css
gulp.task('styles', function () {
    return gulp.src('./Content/scss/**/*.scss')
      .pipe(sass({
          includePaths: [
            govuk_frontend_toolkit_root,
            govuk_elements_sass_root
          ]
      }).on('error', sass.logError))
      .pipe(gulp.dest(buildDir + 'css'));
});

gulp.task('copy', function() {
    var pipe = gulp.src(buildDir);

    for (var i = 0; i < outputPaths.length; i++) {
        pipe.pipe($.copy(outputPaths[i], options));
    }

    return pipe;
})