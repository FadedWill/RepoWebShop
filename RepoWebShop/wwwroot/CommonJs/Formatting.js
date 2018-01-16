﻿function statusCode2DisplayValue(code) {
    switch (code) {
        case "approved":
            return "Aprobado";
        case "pending":
            return "Pendiente";
        case "in_process":
            return "En proceso";
        case "rejected":
            return "Rechazado";
        case "draft":
            return "Sin confirmar";
        case "reservation":
            return "Reserva";
        default:
            return "No disponible";
    }
}
function formatDate(date) {
    var momentDate = moment(date);
    var result = '';
    if (momentDate.isValid()) {
        result = '<div style="display: none;">' + momentDate.format('x') + '</div>';
        result += '<div>';
        result += momentDate.format('dddd') + ' ' + (momentDate.format('DD'));
        result += '<br/>' + momentDate.format('hh:mm a');
        result += '<br/>' + momentDate.format('MMMM');
        result += '<br/>' + momentDate.year();
        result += '</div>';
        return result;
    }
    return '';
}