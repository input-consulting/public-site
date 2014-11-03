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
    $(".frontpicture").fadeIn(500);
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
                }, 200);
        }
    }
    else
    {
        $('.navbar').fadeOut(1000);
    }
});

jQuery(document).ready(function() {
    jQuery('.post').addClass("hidden").viewportChecker({
        classToAdd: 'visible animated bounceInLeft fadeIn',
        offset: 200
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


/*!
 * jQuery scrollify
 * Version 0.1.3
 *
 * Requires:
 * - jQuery 1.6 or higher
 *
 * https://github.com/lukehaas/Scrollify
 *
 * Copyright 2014, Luke Haas
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
(function ($,window,document,undefined) {
    "use strict";
    var heights = [],
        names = [],
        elements = [],
        index = 0,
        currentHash = window.location.hash,
        hasLocation = false,
        timeoutId,
        top = $(window).scrollTop(),
        scrollable = false,
        settings = {
            //section should be an identifier that is the same for each section
            section: "section",
            sectionName: "section-name",
            easing: "easeOutExpo",
            scrollSpeed: 1100,
            offset : 0,
            scrollbars: true,
            axis:"y",
            target:"html,body",
            touchExceptions:"a",
            before:function() {},
            after:function() {}
        };
    $.scrollify = function(options) {

        function animateScroll(index) {

            if(names[index]) {
                settings.before(index,elements);
                if(settings.sectionName) {
                    window.location.hash = names[index];
                }

                $(settings.target).stop().animate({
                    scrollTop: heights[index]
                }, settings.scrollSpeed,settings.easing);

                $(settings.target).promise().done(function(){settings.after(index,elements);});
            }
        }
        var manualScroll = {
            handleMousedown:function() {
                scrollable = false;
            },
            handleMouseup:function() {
                scrollable = true;
            },
            handleScroll:function() {

                if(timeoutId){
                    clearTimeout(timeoutId);
                }
                timeoutId = setTimeout(function(){
                    top = $(window).scrollTop();
                    if(scrollable==false) {
                        return false;
                    }
                    scrollable = false;

                    var i =1,
                        max = heights.length,
                        closest = 0,
                        prev = Math.abs(heights[0] - top),
                        diff;
                    for(;i<max;i++) {
                        diff = Math.abs(heights[i] - top);

                        if(diff < prev) {
                            prev = diff;
                            closest = i;
                        }
                    }
                    index = closest;
                    animateScroll(closest);
                }, 200);
            },
            wheelHandler:function(e,delta) {

                e.preventDefault();

                delta = delta || -e.originalEvent.detail / 3 || e.originalEvent.wheelDelta / 120;

                if(timeoutId){
                    clearTimeout(timeoutId);
                }
                timeoutId = setTimeout(function(){

                    //if(!(index==heights.length-1 && ((index-delta) % (heights.length)==0))) {
                    //index = (index-delta) % (heights.length);
                    //}

                    if(delta<0) {
                        if(index<heights.length-1) {
                            index++;
                        }
                    } else if(delta>0) {
                        if(index>0) {
                            index--;
                        }
                    }

                    if(index>=0) {
                        animateScroll(index);
                    } else {
                        index = 0;
                    }
                },25);
            },
            keyHandler:function(e) {
                e.preventDefault();
                if(e.keyCode==38) {
                    if(index>0) {
                        index--;
                    }
                    animateScroll(index);
                } else if(e.keyCode==40) {
                    if(index<heights.length-1) {
                        index++;
                    }
                    animateScroll(index);
                }
            },
            init:function() {
                if(settings.scrollbars) {
                    $(window).bind('mousedown', manualScroll.handleMousedown);
                    $(window).bind('mouseup', manualScroll.handleMouseup);
                    $(window).bind('scroll', manualScroll.handleScroll);
                } else {
                    $("body").css({"overflow":"hidden"});
                }

                $(document).bind('DOMMouseScroll mousewheel',manualScroll.wheelHandler);
                $(document).bind('keyup', manualScroll.keyHandler);
            }
        };

        var swipeScroll = {
            touches : {
                "touchstart": {"y":-1},
                "touchmove" : {"y":-1},
                "touchend"  : false,
                "direction" : "undetermined"
            },
            options:{
                "distance" : 30,
                "timeGap" : 800,
                "timeStamp" : new Date().getTime()
            },
            touchHandler: function(event) {
                var touch;
                if (typeof event !== 'undefined'){
                    if($(event.target).parents(settings.touchExceptions).length<1 && $(event.target).is(settings.touchExceptions)===false) {
                        event.preventDefault();
                    }
                    if (typeof event.touches !== 'undefined') {
                        touch = event.touches[0];
                        switch (event.type) {
                            case 'touchstart':
                                swipeScroll.options.timeStamp = new Date().getTime();
                                swipeScroll.touches.touchend = false;
                            case 'touchmove':
                                swipeScroll.touches[event.type].y = touch.pageY;
                                if((swipeScroll.options.timeStamp+swipeScroll.options.timeGap)<(new Date().getTime()) && swipeScroll.touches.touchend == false) {
                                    swipeScroll.touches.touchend = true;
                                    if (swipeScroll.touches.touchstart.y > -1) {

                                        if(Math.abs(swipeScroll.touches.touchmove.y-swipeScroll.touches.touchstart.y)>swipeScroll.options.distance) {

                                            if(swipeScroll.touches.touchstart.y < swipeScroll.touches.touchmove.y) {
                                                if(index>0) {
                                                    index--;
                                                }
                                                animateScroll(index);
                                            } else {
                                                if(index<heights.length-1) {
                                                    index++;
                                                }
                                                animateScroll(index);
                                            }
                                        }
                                    }
                                }
                                break;
                            case 'touchend':
                                if(swipeScroll.touches[event.type]==false) {
                                    swipeScroll.touches[event.type] = true;
                                    if (swipeScroll.touches.touchstart.y > -1) {

                                        if(Math.abs(swipeScroll.touches.touchmove.y-swipeScroll.touches.touchstart.y)>swipeScroll.options.distance) {

                                            if(swipeScroll.touches.touchstart.y < swipeScroll.touches.touchmove.y) {
                                                if(index>0) {
                                                    index--;
                                                }
                                                animateScroll(index);
                                            } else {
                                                if(index<heights.length-1) {
                                                    index++;
                                                }
                                                animateScroll(index);
                                            }
                                        }

                                    }
                                }
                            default:
                                break;
                        }
                    }
                }
            },
            init: function() {
                if (document.addEventListener) {
                    document.addEventListener('touchstart', swipeScroll.touchHandler, false);
                    document.addEventListener('touchmove', swipeScroll.touchHandler, false);
                    document.addEventListener('touchend', swipeScroll.touchHandler, false);
                }
            }
        };
        if(typeof options === 'string') {
            var z = names.length;
            for(;z>=0;z--) {
                if(typeof arguments[1] === 'string') {
                    if (names[z]==arguments[1]) {
                        index = z;
                        animateScroll(z);
                    }
                } else {
                    if(z==arguments[1]) {
                        index = z;
                        animateScroll(z);
                    }
                }


            }
        } else {
            settings = $.extend(settings, options);

            $(settings.section).each(function(i){
                if(i>0) {
                    heights[i] = $(this).offset().top + settings.offset;
                } else {
                    heights[i] = $(this).offset().top;
                }
                if(settings.sectionName && $(this).data(settings.sectionName)) {
                    names[i] = "#" + $(this).data(settings.sectionName).replace(/ /g,"-");
                } else {
                    names[i] = "#" + (i + 1);
                }


                elements[i] = $(this);

                if(currentHash==names[i]) {
                    index = i;
                    hasLocation = true;

                }
            });


            if(hasLocation==false && settings.sectionName) {
                window.location.hash = names[0];
            } else {
                animateScroll(index);
            }

            manualScroll.init();
            swipeScroll.init();
        }
    };

}(jQuery,this,document));

/*
 * jQuery Easing v1.3 - http://gsgd.co.uk/sandbox/jquery/easing/
 *
 * Uses the built in easing capabilities added In jQuery 1.1
 * to offer multiple easing options
 *
 * TERMS OF USE - EASING EQUATIONS
 *
 * Open source under the BSD License.
 *
 * Copyright Ã‚Â© 2001 Robert Penner
 * All rights reserved.
 *
 * TERMS OF USE - jQuery Easing
 *
 * Open source under the BSD License.
 *
 * Copyright Ã‚Â© 2008 George McGinley Smith
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list
 * of conditions and the following disclaimer in the documentation and/or other materials
 * provided with the distribution.
 *
 * Neither the name of the author nor the names of contributors may be used to endorse
 * or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 *  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 *  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
 *  GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 *  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
jQuery.easing.jswing = jQuery.easing.swing;
jQuery.extend(jQuery.easing, {
    def: "easeOutQuad",
    swing: function(e, f, a, h, g) {
        return jQuery.easing[jQuery.easing.def](e, f, a, h, g)
    },
    easeInQuad: function(e, f, a, h, g) {
        return h * (f /= g) * f + a
    },
    easeOutQuad: function(e, f, a, h, g) {
        return -h * (f /= g) * (f - 2) + a
    },
    easeInOutQuad: function(e, f, a, h, g) {
        if ((f /= g / 2) < 1) {
            return h / 2 * f * f + a
        }
        return -h / 2 * ((--f) * (f - 2) - 1) + a
    },
    easeInCubic: function(e, f, a, h, g) {
        return h * (f /= g) * f * f + a
    },
    easeOutCubic: function(e, f, a, h, g) {
        return h * ((f = f / g - 1) * f * f + 1) + a
    },
    easeInOutCubic: function(e, f, a, h, g) {
        if ((f /= g / 2) < 1) {
            return h / 2 * f * f * f + a
        }
        return h / 2 * ((f -= 2) * f * f + 2) + a
    },
    easeInQuart: function(e, f, a, h, g) {
        return h * (f /= g) * f * f * f + a
    },
    easeOutQuart: function(e, f, a, h, g) {
        return -h * ((f = f / g - 1) * f * f * f - 1) + a
    },
    easeInOutQuart: function(e, f, a, h, g) {
        if ((f /= g / 2) < 1) {
            return h / 2 * f * f * f * f + a
        }
        return -h / 2 * ((f -= 2) * f * f * f - 2) + a
    },
    easeInQuint: function(e, f, a, h, g) {
        return h * (f /= g) * f * f * f * f + a
    },
    easeOutQuint: function(e, f, a, h, g) {
        return h * ((f = f / g - 1) * f * f * f * f + 1) + a
    },
    easeInOutQuint: function(e, f, a, h, g) {
        if ((f /= g / 2) < 1) {
            return h / 2 * f * f * f * f * f + a
        }
        return h / 2 * ((f -= 2) * f * f * f * f + 2) + a
    },
    easeInSine: function(e, f, a, h, g) {
        return -h * Math.cos(f / g * (Math.PI / 2)) + h + a
    },
    easeOutSine: function(e, f, a, h, g) {
        return h * Math.sin(f / g * (Math.PI / 2)) + a
    },
    easeInOutSine: function(e, f, a, h, g) {
        return -h / 2 * (Math.cos(Math.PI * f / g) - 1) + a
    },
    easeInExpo: function(e, f, a, h, g) {
        return (f == 0) ? a : h * Math.pow(2, 10 * (f / g - 1)) + a
    },
    easeOutExpo: function(e, f, a, h, g) {
        return (f == g) ? a + h : h * (-Math.pow(2, -10 * f / g) + 1) + a
    },
    easeInOutExpo: function(e, f, a, h, g) {
        if (f == 0) {
            return a
        }
        if (f == g) {
            return a + h
        }
        if ((f /= g / 2) < 1) {
            return h / 2 * Math.pow(2, 10 * (f - 1)) + a
        }
        return h / 2 * (-Math.pow(2, -10 * --f) + 2) + a
    },
    easeInCirc: function(e, f, a, h, g) {
        return -h * (Math.sqrt(1 - (f /= g) * f) - 1) + a
    },
    easeOutCirc: function(e, f, a, h, g) {
        return h * Math.sqrt(1 - (f = f / g - 1) * f) + a
    },
    easeInOutCirc: function(e, f, a, h, g) {
        if ((f /= g / 2) < 1) {
            return -h / 2 * (Math.sqrt(1 - f * f) - 1) + a
        }
        return h / 2 * (Math.sqrt(1 - (f -= 2) * f) + 1) + a
    },
    easeInElastic: function(f, h, e, l, k) {
        var i = 1.70158;
        var j = 0;
        var g = l;
        if (h == 0) {
            return e
        }
        if ((h /= k) == 1) {
            return e + l
        }
        if (!j) {
            j = k * 0.3
        }
        if (g < Math.abs(l)) {
            g = l;
            var i = j / 4
        } else {
            var i = j / (2 * Math.PI) * Math.asin(l / g)
        }
        return -(g * Math.pow(2, 10 * (h -= 1)) * Math.sin((h * k - i) * (2 * Math.PI) / j)) + e
    },
    easeOutElastic: function(f, h, e, l, k) {
        var i = 1.70158;
        var j = 0;
        var g = l;
        if (h == 0) {
            return e
        }
        if ((h /= k) == 1) {
            return e + l
        }
        if (!j) {
            j = k * 0.3
        }
        if (g < Math.abs(l)) {
            g = l;
            var i = j / 4
        } else {
            var i = j / (2 * Math.PI) * Math.asin(l / g)
        }
        return g * Math.pow(2, -10 * h) * Math.sin((h * k - i) * (2 * Math.PI) / j) + l + e
    },
    easeInOutElastic: function(f, h, e, l, k) {
        var i = 1.70158;
        var j = 0;
        var g = l;
        if (h == 0) {
            return e
        }
        if ((h /= k / 2) == 2) {
            return e + l
        }
        if (!j) {
            j = k * (0.3 * 1.5)
        }
        if (g < Math.abs(l)) {
            g = l;
            var i = j / 4
        } else {
            var i = j / (2 * Math.PI) * Math.asin(l / g)
        } if (h < 1) {
            return -0.5 * (g * Math.pow(2, 10 * (h -= 1)) * Math.sin((h * k - i) * (2 * Math.PI) / j)) + e
        }
        return g * Math.pow(2, -10 * (h -= 1)) * Math.sin((h * k - i) * (2 * Math.PI) / j) * 0.5 + l + e
    },
    easeInBack: function(e, f, a, i, h, g) {
        if (g == undefined) {
            g = 1.70158
        }
        return i * (f /= h) * f * ((g + 1) * f - g) + a
    },
    easeOutBack: function(e, f, a, i, h, g) {
        if (g == undefined) {
            g = 1.70158
        }
        return i * ((f = f / h - 1) * f * ((g + 1) * f + g) + 1) + a
    },
    easeInOutBack: function(e, f, a, i, h, g) {
        if (g == undefined) {
            g = 1.70158
        }
        if ((f /= h / 2) < 1) {
            return i / 2 * (f * f * (((g *= (1.525)) + 1) * f - g)) + a
        }
        return i / 2 * ((f -= 2) * f * (((g *= (1.525)) + 1) * f + g) + 2) + a
    },
    easeInBounce: function(e, f, a, h, g) {
        return h - jQuery.easing.easeOutBounce(e, g - f, 0, h, g) + a
    },
    easeOutBounce: function(e, f, a, h, g) {
        if ((f /= g) < (1 / 2.75)) {
            return h * (7.5625 * f * f) + a
        } else {
            if (f < (2 / 2.75)) {
                return h * (7.5625 * (f -= (1.5 / 2.75)) * f + 0.75) + a
            } else {
                if (f < (2.5 / 2.75)) {
                    return h * (7.5625 * (f -= (2.25 / 2.75)) * f + 0.9375) + a
                } else {
                    return h * (7.5625 * (f -= (2.625 / 2.75)) * f + 0.984375) + a
                }
            }
        }
    },
    easeInOutBounce: function(e, f, a, h, g) {
        if (f < g / 2) {
            return jQuery.easing.easeInBounce(e, f * 2, 0, h, g) * 0.5 + a
        }
        return jQuery.easing.easeOutBounce(e, f * 2 - g, 0, h, g) * 0.5 + h * 0.5 + a
    }
});


$(document).ready(function(){
    $(function() {
        $.scrollify({
            section : "section.scrollsection",
            easing: "easeOutExpo",
            scrollSpeed: 1100,
            offset : 0,
            scrollbars: true,
            before:function() {},
            after:function() {}
        });
    });
});

function goToSection(a) {
    $.scrollify("move",a);
}
