// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const $player = $('div.player');
$(document).ready(function () {
	//
	$('div.plan-st2').click(function (e) {
		e.preventDefault();
		removeAllScore();
		$that = $(this);
		$that.addClass('active-st2');
		var score = $that.find('h1').data('scoring');
		$player.first().find('div.player-score').remove();
		$player.first().append('<div class="player-score">' + score + '</div>');
		//
	});
});

function removeAllScore() {
	$('div.scores div.plan-st2').removeClass('active-st2');
}