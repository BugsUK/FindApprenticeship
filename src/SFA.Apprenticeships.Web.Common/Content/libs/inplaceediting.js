  if( !$('html.no-js').length ) {
    $( '[data-editable]' ).each( appendTools );
    $( 'body' ).delegate( '.adder', 'click', focusField );
    $( 'body' ).delegate( '.editor', 'click', focusField );
    $( '[data-editable] input, [data-editable] textarea' ).on( 'focus', removeEmpty );
    $( '[data-editable] input, [data-editable] textarea' ).on( 'blur', isFieldEmpty );
    $( '[data-editable] textarea' ).each(function () {
      this.setAttribute( 'style', 'height:' + ( this.scrollHeight ) + 'px;overflow-y:hidden;' );
    }).on('input', autoResize );
  }

  //append tools
  function appendTools(){
    var $this = $( this ),
        $field = $this, //.parent(),
        $aLabel = $this.find( 'label' ).text(),
        $adder,
        $editor;
    if ( !$this.val() ) {
      $this.addClass( 'empty' );
    }
    $adder = $( 
      '<div class="adder" style="width:' 
      + $field.innerWidth() 
      + 'px; height: ' 
      + $field.innerHeight() 
      + 'px; line-height: ' 
      + $field.innerHeight() 
      + 'px;"><span>' + $aLabel + '</span> <span class="bold-small"></span></div>' 
    );
    $editor = $( 
      '<div class="editor" style="width:' 
      + $field.innerWidth() 
      + 'px;"><span>edit</span></div>' 
    );
    $this.append( $adder ).append( $editor );
  };

  // auto adjust the height of
  function autoResize() {
    this.style.height = 'auto';
    this.style.height = (this.scrollHeight) + 'px';
  };

  // pass focus to proper field
  function focusField( event ){
    var $element = $( event.currentTarget );
    $element.parents( '[data-editable]' ).addClass('edited');
    $element.parent().find( 'input, textarea, select' ).trigger('focus');
  };

  // remove empty class for editing
  function removeEmpty( event ){
    var $element = $( event.currentTarget );
    $element.parents( '[data-editable]' ).removeClass('empty');
    $element.parents( '[data-editable]' ).removeClass('full');
  };

  // check if empty and add proper class ( empty or full )
  function isFieldEmpty( event ){
    var $element = $( event.currentTarget );
    $element.parents( '[data-editable]' ).removeClass('edited');
    if( $element.val() === "" ) {
      $element.parents( '[data-editable]' ).addClass('empty');
    } else {
      $element.parents( '[data-editable]' ).addClass('full');
    }
  };