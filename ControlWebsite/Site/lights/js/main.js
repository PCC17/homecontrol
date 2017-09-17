$("#sender").click(function() {
    var msg = "000000";
    msg = msg.concat( $('#amount').val());
    console.log(msg)
    var myAjaxConfig = {
                method: "POST",
                url: "http://api.lime-tree.eu/light/control.php",
                data: {'token':'123321', 'name':'Schlafzimmer-Paul', 'cmd':msg},
                success: function(response){
                    //var obj = jQuery.parseJSON( response);
                      jQuery('<div/>', {
                        id: 'res',
                        class: 'glyphicon glyphicon-ok',
                        style: 'font-size: 2em'
                    }).appendTo('#glyph');
                },
                error: function(response){
                }
            };
            $.ajax(myAjaxConfig);
});