/// <binding AfterBuild='default' />
'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass');
var gutil = require('gulp-util');
var copy = require('gulp-copy');

var repo_root = __dirname + '/';
var govuk_frontend_toolkit_root = repo_root + 'node_modules/govuk_frontend_toolkit/stylesheets'; // 1.
var govuk_elements_sass_root = repo_root + 'node_modules/govuk-elements-sass/public/sass';       // 2.

var buildDir = './build/';

var outputPaths = [
    repo_root + '../SFA.Apprenticeships.Web.Recruit/Content/_assets/',
    repo_root + '../SFA.Apprenticeships.Web.Manage/Content/_assets/',
    repo_root + '../SFA.Apprenticeships.Web.Candidate/Content/_assets/'
];

gulp.task('default', ['styles', 'merge-base', 'copy']);

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

gulp.task('merge-base', function() {
    gulp.src(repo_root + 'node_modules/govuk_template_ejs/assets/images/**/*')
        .pipe(gulp.dest(buildDir + 'img'));
    gulp.src(repo_root + 'node_modules/govuk_frontend_toolkit/images/**/*')
        .pipe(gulp.dest(buildDir + 'img'));
    gulp.src(repo_root + 'node_modules/govuk_template_ejs/assets/stylesheets/**/*')
        .pipe(gulp.dest(buildDir + 'css'));
    return gulp.src(repo_root + 'node_modules/govuk_template_ejs/assets/javascripts/**/*')
        .pipe(gulp.dest(buildDir + 'js'));
});

gulp.task('copy', function() {
    var pipe = gulp.src(buildDir + '**/*');

    for (var i = 0; i < outputPaths.length; i++) {
        pipe.pipe(gulp.dest(outputPaths[i]));
    }

    return pipe;
});