$("#gate").click(function() {
  if( $('#res').length )         // use this if you are using id to check
{
     $('#res').remove;
}
    var myAjaxConfig = {
                method: "POST",
                url: "http://gate.lime-tree.eu/index.php",
                data: "token=5ebe2294ecd0e0f08eab7690d2a6ee69",
                success: function(response){
                    //var obj = jQuery.parseJSON( response);
                      jQuery('<div/>', {
                        id: 'res',
                        class: 'glyphicon glyphicon-ok',
                        style: 'font-size: 2em'
                    }).appendTo('#glyph');
                },
                error: function(response){
                    //var obj = jQuery.parseJSON( response );
                      jQuery('<span/>', {
                        id: 'res',
                        class: 'glyphicon glyphicon-remove',
                        style: 'font-size: 2em'
                    }).appendTo('#glyph');
                }
            };
            $.ajax(myAjaxConfig);
});