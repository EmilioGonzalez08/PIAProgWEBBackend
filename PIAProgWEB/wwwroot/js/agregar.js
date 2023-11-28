function filterProducts(subcategory) {
    // Hide all rows
    $('table tbody tr').hide();

    // Show rows with the selected subcategory
    $('table tbody tr[data-subcategory="' + subcategory + '"]').show();
}

$(document).ready(function () {
    $('.add-to-cart-button').on('click', function () {
        var productId = $(this).data('product-id');
        var currentRow = $(this).closest('tr');
        var selectedSize = currentRow.find('select[name="selectedSize"]');
        var selectedQuantity = parseFloat(currentRow.find('select[name="selectedQuantity"]').val());

        // Obtener la cantidad disponible para la talla seleccionada
        var availableQuantities = currentRow.find('select[name="selectedSize"] option')
            .map(function () { return { size: $(this).val(), quantity: $(this).data('available-quantity') }; })
            .get();

        // Ajustar el máximo de la cantidad al disponible para la talla seleccionada
        var selectedSizeValue = selectedSize.val();
        var selectedSizeAvailability = availableQuantities.find(q => q.size === selectedSizeValue);
        selectedSize.attr('max', selectedSizeAvailability.quantity);

        console.log('Selected Quantity:', selectedQuantity);
        console.log('Available Quantity for selected size:', selectedSizeAvailability.quantity);

        if (!isNaN(selectedQuantity)) {
            if (selectedQuantity <= selectedSizeAvailability.quantity) {
                $.ajax({
                    url: '/Carrito/AddToCart',
                    method: 'POST',
                    data: {
                        productId: productId,
                        selectedSize: selectedSizeValue,
                        selectedQuantity: selectedQuantity
                    },
                    success: function (result) {
                        console.log(result);
                        if (result.success) {
                            alert('Product added to cart successfully!');
                        } else {
                            alert('Error adding product to cart. Please try again.');
                        }
                    },
                    error: function () {
                        alert('Error: Unable to add product to cart');
                    }
                });
            } else {
                alert('Selected quantity exceeds available quantity. Please try again.');
            }
        } else {
            alert('Invalid quantity. Please try again.');
        }
    });

    // Agregar un manejador de eventos para cambiar el máximo de la cantidad al cambiar la talla
    $('select[name="selectedSize"]').on('change', function () {
        var selectedSizeValue = $(this).val();
        var selectedSizeAvailability = availableQuantities.find(q => q.size === selectedSizeValue);
        $(this).siblings('div').find('select[name="selectedQuantity"]').attr('max', selectedSizeAvailability.quantity);
    });
});