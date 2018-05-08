// Write your JavaScript code.

/// Sets the current page in the nav as active
$('a[href="' + this.location.pathname + '"]').parents('li,ul').addClass('active');