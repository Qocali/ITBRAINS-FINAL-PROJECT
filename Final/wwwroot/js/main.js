(function ($) {
"use strict";  
    
/*------------------------------------
	Sticky Menu 
--------------------------------------*/
 var windows = $(window);
    var stick = $(".header-sticky");
	windows.on('scroll',function() {    
		var scroll = windows.scrollTop();
		if (scroll < 5) {
			stick.removeClass("sticky");
		}else{
			stick.addClass("sticky");
		}
	});  
/*------------------------------------
	jQuery MeanMenu 
--------------------------------------*/
	$('.main-menu nav').meanmenu({
		meanScreenWidth: "767",
		meanMenuContainer: '.mobile-menu'
	});
    
    
    /* last  2 li child add class */
    $('ul.menu>li').slice(-2).addClass('last-elements');
/*------------------------------------
	Owl Carousel
--------------------------------------*/
    $('.slider-owl').owlCarousel({
        loop:true,
        nav:true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 2500,
        navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsive:{
            0:{
                items:1
            },
            768:{
                items:1
            },
            1000:{
                items:1
            }
        }
    });
    //$(document).on('click', '#Mc-embedded-subscribe2', function () {
    //    $.ajax({
    //        url: "/Home/SubScribe/",
    //        type: "Get",
    //        data: {
    //            "email": $("#mce-EMAIL").val(),
    //        },
    //        success: function (res) {
    //            $("#Pnim").empty();
    //            $("#Pnim").append(res);
               
    //        }
    //    })
    //})
    //$(document).on('click', '.reply-btn', function () {
    //    $.ajax({
    //        url: "/Contact/Send/",
    //        type: "Get",
    //        data: {
    //            "message": $("#message").val(),
    //            "email": $("#email").val(),
    //            "subject": $("#subject").val(),
    //            "name": $("#name").val(),
    //        },
    //        success: function (res) {
    //            $("#Pnim").empty();
    //            $("#Pnim").append(res);
    //            if ($("#email").val() != null) {
    //                $("#message").empty();
    //                $("#email").empty();
    //                $("#subject").empty();
    //                $("#name").empty();
    //            };
    //        }
    //    })
    //})
   
    //$(document).on('keyup', '#teachersearch', function () {
    //    $.ajax({
    //        url: "/Course/GoSearch/",
    //        type: "Get",
    //        data: {
    //            "search": $("#teachersearch").val(),
    //        },
    //        success: function (res) {
    //            $("#Try").empty(res);
    //            $("#Try").append(res);
    //            }
               
    //        }
    //    })
    //})
    $('.partner-owl').owlCarousel({
        loop:true,
        nav:true,
        navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsive:{
            0:{
                items:1
            },
            768:{
                items:3
            },
            1000:{
                items:5
            }
        }
    });  

    $('.testimonial-owl').owlCarousel({
        loop:true,
        nav:true,
        navText:['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsive:{
            0:{
                items:1
            },
            768:{
                items:1
            },
            1000:{
                items:1
            }
        }
    });
/*------------------------------------
	Video Player
--------------------------------------*/
    $('.video-popup').magnificPopup({
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,
        zoom: {
            enabled: true,
        }
    });
    
    $('.image-popup').magnificPopup({
        type: 'image',
        gallery:{
            enabled:true
        }
    }); 
/*----------------------------
    Wow js active
------------------------------ */
    new WOW().init();
/*------------------------------------
	Scrollup
--------------------------------------*/
    $.scrollUp({
        scrollText: '<i class="fa fa-angle-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade'
    });
/*------------------------------------
	Nicescroll
--------------------------------------*/
     $('body').scrollspy({ 
            target: '.navbar-collapse',
            offset: 95
        });
$(".notice-left").niceScroll({
            cursorcolor: "#EC1C23",
            cursorborder: "0px solid #fff",
            autohidemode: false,
            
        });

})(jQuery);	