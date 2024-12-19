// prueba.js

function filterNames() {
    // Obtener el texto ingresado en el input de búsqueda
    var searchText = document.getElementById('searchPag').value.toLowerCase();

    // Obtener todas las etiquetas <span> con la clase "nav-link-text"
    var names = document.querySelectorAll('.nav-link-text');

    // Variable para indicar si se ha encontrado alguna coincidencia
    var found = false;

    // Iterar sobre cada nombre para ver si coincide con el texto de búsqueda
    names.forEach(function (name) {
        var parentListItem = name.closest('.nav-item'); // Buscar el elemento padre .nav-item más cercano

        // Comprobar si el nombre coincide con el texto de búsqueda
        if (name.textContent.toLowerCase().includes(searchText)) {
            // Si coincide, mostrar el elemento padre y sus elementos ascendentes
            showParentItems(parentListItem);
            found = true; // Indicar que se encontró al menos una coincidencia
        } else {
            // Si no coincide, ocultar el elemento padre y sus elementos descendentes
            hideParentItems(parentListItem);
        }
    });

    // Si no se encontró ninguna coincidencia, mostrar todos los elementos
    if (!found) {
        names.forEach(function (name) {
            var parentListItem = name.closest('.nav-item');
            showParentItems(parentListItem);
        });
    }
}

// Función para mostrar el elemento padre y sus elementos ascendentes
function showParentItems(element) {
    while (element) {
        element.style.display = ''; // Mostrar el elemento
        if (element.classList.contains('parent-wrapper')) {
            element.previousElementSibling.classList.remove('collapsed'); // Mostrar el ícono de la flecha
        }
        element = element.parentElement.closest('.nav-item'); // Moverse al siguiente elemento padre
    }
}

// Función para ocultar el elemento padre y sus elementos descendentes
function hideParentItems(element) {
    if (!element) return;
    element.style.display = 'none'; // Ocultar el elemento
    if (element.classList.contains('parent-wrapper')) {
        element.previousElementSibling.classList.add('collapsed'); // Ocultar el ícono de la flecha
    }
    var childItems = element.querySelectorAll('.nav-item'); // Obtener los elementos hijos
    childItems.forEach(function (child) {
        hideParentItems(child); // Ocultar los elementos descendentes
    });
    //location.reload()
}

