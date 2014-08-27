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
        init: init,
    };

}());


$(document).ready(function () {
    Articles.articleLists.init();
});