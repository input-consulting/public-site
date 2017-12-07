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
        target: '#nav',
        offset: 69
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
        posi = $('#nav').position().top > 0 ? posi - $('#nav').height() : posi;
        $('body,html').animate({
            scrollTop: posi - 40
        }, 600);
    });    

    // $('#nav').on('affixed.bs.affix', function () {
    //   $('body').scrollspy('refresh');
    // });    
});
