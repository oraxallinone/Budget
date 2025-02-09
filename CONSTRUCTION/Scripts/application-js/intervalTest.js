$(document).ready(function () {
    //window.setInterval(function () {
    //    console.log('one');
    //    $('#dis').append('<table><tr><td>hi</td></tr></table>')
    //}, 5000);

    $('#receiptSubmit').click(function () {
        $('#one').val('ooo');


        swal({
            title: "Are you sure?",
            text: "You will not be able to recover this imaginary file!",
            icon: "warning",
            buttons: [
              'No, cancel it!',
              'Yes, I am sure!'
            ],
            dangerMode: true,
        }).then(function (isConfirm) {
            if (isConfirm) {
                alert('ho');
            } else {
                
            }
        })








        //swal({
        //    title: 'Are you sure?',
        //    text: "It will permanently deleted !",
        //    type: 'warning',
        //    showCancelButton: true,
        //    confirmButtonColor: '#3085d6',
        //    cancelButtonColor: '#d33',
        //    confirmButtonText: 'Yes, delete it!'
        //}).then(function () {
        //    swal(
        //      'Deleted!',
        //      'Your file has been deleted.',
        //      'success'
        //    );
        //})

    })



    $('#receiptSubmitqq').click(function () {

        $('#one').val('ooo');
        swal({
            title: 'Are you sure?',
            text: "It will permanently deleted !",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(function () {
            swal(
              'Deleted!',
              'Your file has been deleted.',
              'success'
            );
        })

    });



   
});


