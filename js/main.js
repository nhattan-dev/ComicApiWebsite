
(function ($) {
    "use strict";


    /*==================================================================
   [ Focus input ]*/
    $('.input100').each(function () {
        $(this).on('blur', function () {
            if ($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })
    })


    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');

    $('.validate-form').on('submit', function () {
        var check = true;

        for (var i = 0; i < input.length; i++) {
            if (validate(input[i]) == false) {
                showValidate(input[i]);
                check = false;
            }
        }

        return check;
    });


    $('.validate-form .input100').each(function () {
        $(this).focus(function () {
            hideValidate(this);
        });
    });

    function validate(input) {
        if ($(input).attr('type') == 'email' || $(input).attr('name') == 'email') {
            if ($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                return false;
            }
        }
        else {
            if ($(input).val().trim() == '') {
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }



})(jQuery);
.logo-icon{
    width: 50% !important;
}
.detail-image{
    width: 100%;
}
.span-color-white{
    color: rgb(12, 240, 23) !important;
}
.span-color-black{
    color: white !important;
    background-color: rgb(112, 182, 240);
}
.a-padding{
    padding: 5px 5px;
}
.detail-nav{
    background-color: #f7f5f5;
    border-radius: 5px 5px;
    padding-bottom: 30px;
    border: 0px;
    box-shadow: 0 0 1px rgb(0 0 0 / 13%), 0 1px 3px rgb(0 0 0 / 20%);
}
.detail-item{
    border:1px #f8f8f8 groove;
    padding: 25px 20px;
    border: 0px;
    box-shadow: 0 0 1px rgb(0 0 0 / 13%), 0 1px 3px rgb(0 0 0 / 20%);
    margin-top: 5px;
}
.padding-10{
    padding: 10px 10px;
}
.padding-10-r-t{
    padding-right: 10px;
    padding-top: 5px;
}
.padding-10-r{
    padding-right: 10px !important;
}
.padding-10-l{
    padding-left: 10px !important;
}
.padding-10-t{
    padding-top: 10px !important;
}
.list-chapter{
    max-height: 300px;
    margin-bottom: 10px;
    overflow: scroll;
    -webkit-overflow-scrolling: touch;
}
.topAll {
    background-color: #f8f8f8;
}
.sidebar-topAll {
    margin-top: 0;
    border: 1px solid #ececec;
    border-radius: 2px;
    box-shadow: 0 0 1px rgb(0 0 0 / 13%), 0 1px 3px rgb(0 0 0 / 20%);
}
.font-barlow-cond{
    font-family: 'Barlow Condensed', sans-serif;
    font-size: 120%;
}
.sidebar-content{
    padding: 10px 0px;
}
a{
    color: black;
}
.detail-container{
    margin-left: 0px;
}
.font-size-130{
    font-size: 130% !important;
}
.font-size-115{
    font-size: 115% !important;
}
.font-size-150{
    font-size: 150% !important;
}
.color-chapter{
    color: green;
}
.top-item{
    padding: 10px 5px !important;
}
.card-header{
    background-color: rgb(224 217 217);
}

* {box-sizing:border-box}

/* Slideshow container */
.slideshow-container {
  max-width: 1000px;
  position: relative;
  margin: auto;
}

/* Hide the images by default */
.mySlides {
  display: none;
}

/* Next & previous buttons */
.prev, .next {
  cursor: pointer;
  position: absolute;
  top: 50%;
  width: auto;
  margin-top: -22px;
  padding: 16px;
  color: white;
  font-weight: bold;
  font-size: 18px;
  transition: 0.6s ease;
  border-radius: 0 3px 3px 0;
  user-select: none;
}

/* Position the "next button" to the right */
.next {
  right: 0;
  border-radius: 3px 0 0 3px;
}

/* On hover, add a black background color with a little bit see-through */
.prev:hover, .next:hover {
  background-color: rgba(0,0,0,0.8);
}

/* Caption text */
.text {
  color: #f2f2f2;
  font-size: 15px;
  padding: 8px 12px;
  position: absolute;
  bottom: 8px;
  width: 100%;
  text-align: center;
}

/* Number text (1/3 etc) */
.numbertext {
  color: #f2f2f2;
  font-size: 12px;
  padding: 8px 12px;
  position: absolute;
  top: 0;
}

/* The dots/bullets/indicators */
.dot {
  cursor: pointer;
  height: 15px;
  width: 15px;
  margin: 0 2px;
  background-color: #bbb;
  border-radius: 50%;
  display: inline-block;
  transition: background-color 0.6s ease;
}

.active, .dot:hover {
  background-color: #717171;
}

/* Fading animation */
.fade {
  -webkit-animation-name: fade;
  -webkit-animation-duration: 1.5s;
  animation-name: fade;
  animation-duration: 1.5s;
}

@-webkit-keyframes fade {
  from {opacity: .4}
  to {opacity: 1}
}

@keyframes fade {
  from {opacity: .4}
  to {opacity: 1}
}

.slideshow-image{
    height: 300px;
    background-position: center; 
    background-repeat: no-repeat;
    background-size: contain;
}

.slideshow-item{
    padding-top: 10px;
    padding-right: 7px;
    padding-left: 7px;
    max-width: 218px !important;
    max-height: 363px !important;
}
.chapter-item{
    margin: 10px 0px;
    padding: 0px 0px;
}

.chapter-list-item{
    margin-bottom: 6px;
}

.padding-0{
    padding-left: 0px !important;
    padding-right: 0px !important;
}
.margin-0{
    margin: 0px 0px !important;
}
.image-item{
    width: 100% !important;
    height: auto;
}
.background-black{
    background-color: black;
}
.w-65{
    width: 65% !important;
}


/* design by NT */
.list-chapter .chapter-list-item .row:hover{
    background: gainsboro;
}

.comic-content{
    background-color: #f7f5f5 !important;
    padding: 0px 5px;
}

.comic-item{
    border: rgb(238, 236, 236);
    border-style: solid;
    margin: 10px 10px;
}

.comic-header{
    padding-top: 11px;
}

.chapter-item{
    /* border-style: solid; */
    background-color: #f7f5f5;
    padding-left: 4px !important;
}

.chapter-list{
    padding-inline-start: 40px;
    padding-inline-end: 10px;
}

ul{list-style-type:none};