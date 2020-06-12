$(".sidebar-dropdown a").click(function () {
	$(this)
		.toggleClass("open")
		.siblings(".sidebar-submenu")
		.slideToggle();
	$(this)
		.closest(".sidebar-dropdown")
		.toggleClass("open")
		.siblings(".sidebar-dropdown")
		.removeClass("open")
		.find(".sidebar-submenu")
		.slideUp();
	$(this)
		.closest(".sidebar-dropdown")
		.siblings(".sidebar-dropdown")
		.find("a.dropdown-link")
		.removeClass("open");
});
$(".sidebar-dropdown a.active")
	.parents(".sidebar-submenu")
	.addClass("open")
	.slideDown();

$(".sidebar-dropdown a.active")
	.parents(".sidebar-submenu")
	.addClass("open")
	.siblings("a.dropdown-link")
	.addClass("open");

$(".sidebar-dropdown a.active")
	.parents(".sidebar-dropdown")
	.addClass("open");