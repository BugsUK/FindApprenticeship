/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

    config.toolbar = [
        { name: 'paragraph', items: ['NumberedList','BulletedList'] },
    { name: 'styles', items: ['Format'] }
    ];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript,Stylescombo';

	// Set the most common block elements.
	config.format_tags = 'p;h3';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';
    //config.disableNativeSpellChecker = false;
	config.pasteFromWordRemoveFontStyles = false;
	config.extraPlugins = 'pastefromword';
};
