var Articles = Articles || {};

Articles.articleLists = (function () {
    'use strict';

    var articleSelector = 'article';

    var articleClickEvent = function () {
        $(articleSelector).click(function (e) {
            var href = $(this).data('href');
            if (typeof href !== "undefined") {
                e.stopPropagation();
                window.location.href = href;
            }
        });
    };

    var init = function () {
        articleClickEvent();
    };

    return {
        init: init
    };

}());


$(document).ready(function () {
    Articles.articleLists.init();
    $("#container").fadeIn(1500);
    $("#infoi").fadeIn(300);
    $("#infoi").animate(
        {
            'left': '40%'
        }, 1800);
});

//TODO clean this up when haveing new branch

$(window).scroll(function () {
    if($(this).scrollTop() > 600)
    {
        $('.navbar').fadeIn(500);
        if (!$('.navbar').hasClass('fixed'))
        {
            $('.navbar').stop().addClass('fixed').css('top', '-0px').animate(
                {
                    'top': '0px'
                }, 500);
        }
    }
    else
    {
        $('.navbar').fadeOut(1000);
    }
});

jQuery(document).ready(function() {
    jQuery('.post').addClass("hidden").viewportChecker({
        classToAdd: 'visible animated bounceInLeft',
        offset: 200,
        fade:233
    });
});

(function($){
    $.fn.viewportChecker = function(useroptions){
        // Define options and extend with user
        var options = {
            classToAdd: 'visible',
            offset: 100,
            callbackFunction: function(elem){}
        };
        $.extend(options, useroptions);

        // Cache the given element and height of the browser
        var $elem = this,
            windowHeight = $(window).height();

        this.checkElements = function(){
            // Set some vars to check with
            var scrollElem = ((navigator.userAgent.toLowerCase().indexOf('webkit') != -1) ? 'body' : 'html'),
                viewportTop = $(scrollElem).scrollTop(),
                viewportBottom = (viewportTop + windowHeight);

            $elem.each(function(){
                var $obj = $(this);
                // If class already exists; quit
                if ($obj.hasClass(options.classToAdd)){
                    return;
                }

                // define the top position of the element and include the offset which makes is appear earlier or later
                var elemTop = Math.round( $obj.offset().top ) + options.offset,
                    elemBottom = elemTop + ($obj.height());

                // Add class if in viewport
                if ((elemTop < viewportBottom) && (elemBottom > viewportTop)){
                    $obj.addClass(options.classToAdd);

                    // Do the callback function. Callback wil send the jQuery object as parameter
                    options.callbackFunction($obj);
                }
            });
        };

        // Run checkelements on load and scroll
        $(window).scroll(this.checkElements);
        this.checkElements();

        // On resize change the height var
        $(window).resize(function(e){
            windowHeight = e.currentTarget.innerHeight;
        });
    };
})(jQuery);