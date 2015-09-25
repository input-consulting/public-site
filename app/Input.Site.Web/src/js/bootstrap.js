$(document).ready(function () {
    
    var isMobile = function(agent) {
      return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(agent);
    };
    
    /* affix the navbar after scroll below header */
    $('#nav').affix({
        offset: {
            top: $('header').height() - $('#nav').height()
        }
    });

    /* highlight the top nav as scrolling occurs */
    $('body').scrollspy({
        target: '#nav'
    })

    /* smooth scrolling for scroll to top */
    $('.scroll-top').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 1000);
    });

    /* smooth scrolling for nav sections */
    $('#nav .navbar-nav li>a').click(function () {
        var link = $(this).attr('href');
        var posi = $(link).offset().top;
        $('body,html').animate({
            scrollTop: posi
        }, 700);
    });


    var isMobile = isMobile(navigator.userAgent);
    var news = $(".news-item");
    
    if ( isMobile ) {
        for (ii = 0, len = news.length; ii < len; ii++) {
            news.eq(ii).css("visibility","visible");
        }
    } else {
        if (news.length > 0) {
            news.eq(0).addClass("wow fadeInLeft");
            if (news.length === 2) {
                news.eq(1).addClass("wow fadeInRight");
            }
        }
    }
    
    new WOW({
          boxClass:     'wow',      // default
          animateClass: 'animated', // default
          offset:       0,          // default
          mobile:       false,       // default
          live:         true        // default
    }).init();
    
});