$(document).ready(function(){
    var resultDOM = $('#verification-id');
    var randomID = Math.random().toString(36).slice(-8);
    resultDOM.text(randomID)
 });
