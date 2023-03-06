// JavaScript Document

//include共用頁面
$(function ($) {
	$.include = function (url) {
		$.ajax({
			url: url,
			async: false,
			success: function (result) {
				document.write(result);
			}
		});
	};
}(jQuery));

//搜尋下拉
$(document).ready(function(){
	$(".search_icon").click(function(){
		var collapsed=$(this).find('i').hasClass('fa-search-minus');
		$(".search_menu").slideToggle();	
		$('.search_icon').find('i').removeClass('fa-search');
		$('.search_icon').find('i').addClass('fa-search-minus');
		if(collapsed)
			$(this).find('i').toggleClass('fa-search-minus fa-2x fa-search fa-2x')
	});
});

//問卷樣式
function toggleChoice() {
eval("trText").style.display = (eval("formElementText").checked ? '' : 'none');
eval("trTextarea").style.display = (eval("formElementTextarea").checked ? '' : 'none');
eval("trOther").style.display = (!eval("formElementText").checked && !eval("formElementTextarea").checked ? '' : 'none');
}

//日曆
/*$(function() {
	$('#date_item').datetimepicker({  
			format: 'yyyy/MM/dd ',  
			language: 'en',
	}); 
});*/

//教育訓練訊息通知-查看訊息
$(function(){
	$(".message_view_1").click(function(){
		$(".view_1").slideToggle(300);
		return false;//取消超連結功能，#就不會跳上
	});
	$(".message_view_2").click(function(){
		$(".view_2").slideToggle(300);
		return false;//取消超連結功能，#就不會跳上
	});
});

