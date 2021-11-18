$(document).ready(function(){
    var randomID = Math.random().toString(36).slice(-8);
    document.getElementById("code").setAttribute('value', randomID);
 });
