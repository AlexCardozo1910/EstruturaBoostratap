// Exemplo de JavaScript inicial para desativar envios de formulário, se houver campos inválidos.
(function() {
    'use strict';
    window.addEventListener('load', function() {
    // Pega todos os formulários que nós queremos aplicar estilos de validação Bootstrap personalizados.
    var forms = document.getElementsByClassName('needs-validation');
    // Faz um loop neles e evita o envio
    var validation = Array.prototype.filter.call(forms, function(form) {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
  }, false);
})();

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    if ($(".maskInscricao").length)
        $(".maskInscricao").mask("999.999.999.999")

    if ($(".maskCPF").length)
        $(".maskCPF").mask("999.999.999-99")

    if ($(".maskCNPJ").length)
        $(".maskCNPJ").mask("99.999.999/9999-99")

    if ($(".maskPhone").length) {
        var SPMaskBehavior = function (val) {
            return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
        },
            spOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(SPMaskBehavior.apply({}, arguments), options);
                }
            };

        $('.maskPhone').mask(SPMaskBehavior, spOptions);

    }

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        if ($(".toggler").hasClass("hidden"))
            $('.toggler').removeClass('hidden');
        else
            $('.toggler').addClass('hidden');
    });

    if ($('.dataPicker').length > 0) {
        $('.dataPicker').datetimepicker({
            "allowInputToggle": true,
            "showClose": true,
            "showClear": true,
            "showTodayButton": true,
            "format": "DD/MM/YYYY",
        });
    }

    $("#seleciona-tudo").on('click', function () {
        if ($(this).is(':checked')) {
            $('input:checkbox').prop("checked", true);
        } else {
            $('input:checkbox').prop("checked", false);
        }
    });

    if ($("#chart").length > 0) {

        var options = {
            fill: {
                colors: ['#0077db', '#ffb900']
            },
            legend: {
                markers: {
                    width: 12,
                    height: 12,
                    fillColors: ['#0077db', '#ffb900'],
                    radius: 12,
                },
            },
            chart: {
                width: "100%",
                height: 380,
                type: "bar"
            },
            plotOptions: {
                bar: {
                    horizontal: false
                }
            },
            dataLabels: {
                enabled: false
            },
            stroke: {
                width: 1,
                colors: ["#fff"]
            },
            series: [
                {
                    name: 'CSAT Anterior',
                    data: [44, 55, 41, 64, 22],
                    colors: '#2983FF'
                },
                {
                    name: 'CSAT Atual',
                    data: [53, 32, 33, 52, 13],
                    colors: '#F9C80E'
                }
            ],
            xaxis: {
                categories: [
                    "Geral",
                    "Preço",
                    "Produto",
                    "Atendimento",
                    "Loja"
                ]
            },
            responsive: [
                {
                    breakpoint: 1000,
                    options: {
                        plotOptions: {
                            bar: {
                                horizontal: false
                            }
                        },
                        legend: {
                            position: "bottom"
                        }
                    }
                }
            ]
        };

        var chart = new ApexCharts(document.querySelector("#chart"), options);

        chart.render();
    }
});

function setCamposCadastro() {
    if ($("#PessoaFisica").is(":checked")) {
        $("#CamposGenero").removeClass("hidden");
        $("#DataNascimento").prop("required", true);
        $("#Genero").prop("required", true);
        $("#CpfCnpj").unmask().mask("999.999.999-99");
    }
    else {
        $("#CamposGenero").addClass("hidden");
        $("#DataNascimento").prop("required", false);
        $("#Genero").prop("required", false);
        $("#CpfCnpj").unmask().mask("99.999.999/9999-99");
    }
}

function setIsencao() {
    if ($("#Isento").is(":checked")) {
        $("#RgIe").prop("required", false);
        $("#RgIe").prop("readonly", true);
        $("#RgIe").val("");
    }
    else {
        $("#RgIe").prop("required", true);
        $("#RgIe").prop("readonly", false);
    }
}