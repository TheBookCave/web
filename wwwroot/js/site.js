// Write your JavaScript code.

/// Sets the current page in the navbar as active
$('a[href="' + this.location.pathname + '"]').parents('li,ul').addClass('active');
