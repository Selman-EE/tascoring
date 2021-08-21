/**********************

Author:  WHMCSdes
Template: X-DATA - WMHCS & HTML Web Hosting Template
Version: 1.8
Author URI: whmcsdes.com

***************************/

/*global jQuery, var, $ */

(function ($) {
    'use strict';

    // Layout cog
    $('.icon-cog').on('click', function () {
        if ($(this).css("margin-left") === "200px") {
            $('.box-layout').animate({
                "margin-left": '-=200'
            });
            $('.icon-cog').animate({
                "margin-left": '-=200'
            });
        } else {
            $('.box-layout').animate({
                "margin-left": '+=200'
            });
            $('.icon-cog').animate({
                "margin-left": '+=200'
            });
        }
    });

    //  Green Skin
    $(".green-o").on('click', function () {
        $("link[href*='theme']").attr("href", "css/" + $(this).val() + ".css");
    });

    // Sky Skin
    $(".sky-o").on('click', function () {
        $("link[href*='theme']").attr("href", "css/" + $(this).val() + ".css");
    });

    // Yellow Skin
    $(".yellow-o").on('click', function () {
        $("link[href*='theme']").attr("href", "css/" + $(this).val() + ".css");
    });

    // Violet Skin
    $(".violet-o").on('click', function () {
        $("link[href*='theme']").attr("href", "css/" + $(this).val() + ".css");
    });

    // Layout width
    $(".wide-layout").on('click', function () {
        $(".layout-width").css({
            "width": "100%",
            "margin": "0"
        });
    });

    $(".boxed-layout").on('click', function () {
        $(".layout-width").css({
            "width": "1200px",
            "margin": "50px auto"
        });
    });

    // Pattern Layout
    $(".pattern1").on('click', function () {
        $("body").css("background-image", "url(img/patterns/1.png)");
    });

    $(".pattern2").on('click', function () {
        $("body").css("background-image", "url(img/patterns/2.png)");
    });

    $(".pattern3").on('click', function () {
        $("body").css("background-image", "url(img/patterns/3.png)");
    });

    $(".pattern4").on('click', function () {
        $("body").css("background-image", "url(img/patterns/4.png)");
    });

    $(".pattern5").on('click', function () {
        $("body").css("background-image", "url(img/patterns/5.png)");
    });

    $(".pattern6").on('click', function () {
        $("body").css("background-image", "url(img/patterns/6.png)");
    });

    $(".pattern7").on('click', function () {
        $("body").css("background-image", "url(img/patterns/7.png)");
    });

    $(".pattern8").on('click', function () {
        $("body").css("background-image", "url(img/patterns/8.png)");
    });

    $(".pattern9").on('click', function () {
        $("body").css("background-image", "url(img/patterns/9.png)");
    });

    $(".pattern10").on('click', function () {
        $("body").css("background-image", "url(img/patterns/10.png)");
    });

    // Tooltip operator
    $('[data-toggle="tooltip"]').tooltip();

    // Testimontial Carousel
    $('.owl-carousel').owlCarousel({
        items: 1,
        margin: 0,
        loop: true,
        autoplay: true,
        autoplayTimeout: 5000,
        autoplayHoverPause: true
    });

    // Modern Layout Cadrousel
    $('.modernlyo-owl').owlCarousel({
        items: 1,
        loop: true,
        margin: 0,
        autoplay: true,
        autoplayTimeout: 5000,
        autoplayHoverPause: true
    });

    // TeamWork Carousel
    $('.team-owl').owlCarousel({
        items: 1
    });

    // SmoothScroll
    $('a#m-details,a#m-details2,a#m-details3,a.g-q,a.d-reg,a.r-res,.code-nav ul li a').not('[href="#"]').not('[href="#0"]').click(function (event) {
        if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                event.preventDefault();
                $('html, body').animate({
                    scrollTop: target.offset().top
                }, 1000, function () {
                    var $target = $(target);
                    $target.focus();
                    if ($target.is(":focus")) {
                        return false;
                    } else {
                        $target.attr('tabindex', '-1');
                        $target.focus();
                    }
                });
            }
        }
    });

    // Owl Carousel Shortcode
    $('.owl-carousel-short').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });

    // Landing Page Scroll
    var sections = $('section,.landing-header,.landing-page-footer')
    , nav = $('nav')
    , nav_height = nav.outerHeight();
  
  $(window).on('scroll', function () {
    var cur_pos = $(this).scrollTop();
    
    sections.each(function() {
      var top = $(this).offset().top - nav_height,
          bottom = top + $(this).outerHeight();
      
      if (cur_pos >= top && cur_pos <= bottom) {
        nav.find('a').removeClass('active');
        sections.removeClass('active');
        
        $(this).addClass('active');
        nav.find('a[href="#'+$(this).attr('id')+'"]').addClass('active');
      }
    });
  });
  
  nav.find('a').on('click', function () {
    var $el = $(this)
      , id = $el.attr('href');
    
    $('html, body').animate({
      scrollTop: $(id).offset().top - nav_height
    }, 500);
    
    return false;
  });

})(jQuery);
