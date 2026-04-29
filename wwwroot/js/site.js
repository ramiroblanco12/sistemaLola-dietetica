/* =========================================================
   SITE.JS UNIFICADO - PROYECTO LOLA
   ========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    /* ========================================================
       TOAST GLOBAL (Layout)
       ======================================================== */
    window.mostrarToast = function (mensaje, tipo = "exito") {
        const toast = document.getElementById('toastNotificacion');
        const msg = document.getElementById('toastMensaje');
        const title = document.getElementById('toastTitulo');

        if (!toast || !msg || !title) return;

        msg.textContent = mensaje;

        switch (tipo) {
            case "error":
                toast.style.backgroundColor = "#dc3545";
                title.textContent = "Error";
                break;
            case "info":
                toast.style.backgroundColor = "#2563eb";
                title.textContent = "Info";
                break;
            default:
                toast.style.backgroundColor = "#198754";
                title.textContent = "Listo";
                break;
        }

        toast.classList.add('mostrar');

        setTimeout(() => {
            toast.classList.remove('mostrar');
        }, 3000);
    };


    /* ========================================================
       PROVEEDORES - Contador de notas
       ======================================================== */
    (function () {
        const notas = document.getElementById("Anotacion");
        const counter = document.getElementById("countNotas");

        if (!notas || !counter) return;

        function actualizarContador() {
            counter.textContent = `${notas.value.length} / 250`;
        }

        actualizarContador();
        notas.addEventListener("input", actualizarContador);
    })();


    /* ========================================================
       PRODUCTOS INDEX
       ======================================================== */
    (function () {

        const ctx = document.getElementById("productosCtx");
        if (!ctx) return;

        const urlBuscarPorCodigo = ctx.dataset.urlBuscar;
        const urlDatosEtiqueta = ctx.dataset.urlEtiqueta;

        const txtBuscar = document.getElementById("codigoBarraHidden");
        const fProveedor = document.getElementById("fProveedor");
        const fCategoria = document.getElementById("fCategoria");
        const btnLimpiar = document.getElementById("btnLimpiar");
        const lblResultados = document.getElementById("lblResultados");

        const modalEl = document.getElementById("productoModal");
        const modalContent = document.getElementById("productoModalContent");

        if (!txtBuscar) return;

        function aplicarFiltros() {
            const q = (txtBuscar.value || "").trim().toLowerCase();
            const provId = (fProveedor?.value || "").trim();
            const catId = (fCategoria?.value || "").trim();

            const rows = document.querySelectorAll("tr.prod-row");
            let visibles = 0;

            rows.forEach(r => {
                const nombre = (r.dataset.nombre || "").toLowerCase();
                const barcode = (r.dataset.barcode || "").toLowerCase();
                const proveedorId = (r.dataset.proveedorid || "");
                const categoriaId = (r.dataset.categoriaid || "");

                const matchTexto = !q || nombre.includes(q) || barcode.includes(q);
                const matchProveedor = !provId || proveedorId === provId;
                const matchCategoria = !catId || categoriaId === catId;

                const match = matchTexto && matchProveedor && matchCategoria;

                r.style.display = match ? "" : "none";
                if (match) visibles++;
            });

            if (lblResultados)
                lblResultados.textContent = `${visibles} resultado(s)`;
        }

        txtBuscar.addEventListener("input", aplicarFiltros);
        fProveedor?.addEventListener("change", aplicarFiltros);
        fCategoria?.addEventListener("change", aplicarFiltros);

        btnLimpiar?.addEventListener("click", () => {
            txtBuscar.value = "";
            if (fProveedor) fProveedor.value = "";
            if (fCategoria) fCategoria.value = "";
            aplicarFiltros();
        });

        window.abrirEditar = async function (codigo) {
            try {
                const r = await fetch(`${urlBuscarPorCodigo}?codigo=${encodeURIComponent(codigo)}`);
                if (!r.ok) throw new Error();

                const html = await r.text();
                modalContent.innerHTML = html;
                bootstrap.Modal.getOrCreateInstance(modalEl).show();
            } catch {
                alert("No se pudo abrir el editor.");
            }
        };

        window.ImprimirEtiqueta = async function (productoId) {
            try {
                const r = await fetch(`${urlDatosEtiqueta}?id=${encodeURIComponent(productoId)}`);
                if (!r.ok) {
                    alert("No se encontró el producto");
                    return;
                }

                const data = await r.json();

                const resp = await fetch("http://localhost:5155/print/label", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        printerName: "POS58 Printer",
                        productName: data.nombre,
                        price: data.precio,
                        uniteText: data.descripcion,
                        code: data.codigo
                    })
                });

                if (!resp.ok) {
                    alert("No se pudo imprimir. ¿Está abierto el Print Agent?");
                    return;
                }

                alert("Etiqueta enviada a imprimir ✅");
            } catch (e) {
                console.error(e);
                alert("Error en impresión");
            }
        };

    })();


    /* ========================================================
       PUNTO DE VENTA
       ======================================================== */
    (function () {

        const ctx = document.getElementById("pvCtx");
        if (!ctx) return;

        const urlBuscarProducto = ctx.dataset.urlBuscar;
        const urlConfirmarVenta = ctx.dataset.urlConfirmar;

        const input = document.getElementById('codigoBarraVentaInput');
        const tbody = document.getElementById('detalleVentaBody');
        const totalSpan = document.getElementById('totalVenta');
        const btnConfirmar = document.getElementById('btnConfirmarVenta');
        const medioPagoSelect = document.getElementById('medioPago');

        function focusScanner() {
            // Si hay un modal abierto, no robes foco
            if (document.body.classList.contains('modal-open')) return;

            // Si el usuario está escribiendo en un input/textarea/select, no robes foco
            const active = document.activeElement;
            if (active && ['INPUT', 'TEXTAREA', 'SELECT'].includes(active.tagName)) {
                // PERO: si el active es el scanner, sí está bien
                if (active.id !== 'codigoBarraVentaInput') return;
            }

            input.focus();
        }


        window.addEventListener('load', focusScanner);

        document.addEventListener('click', (e) => {

            // Si clickea en la cantidad, no robar foco
            if (e.target.closest('.input-cantidad')) return;

            // Si clickea dentro de la tabla, no robar foco
            if (e.target.closest('#tablaVenta')) return;

            setTimeout(focusScanner, 0);
        });



        if (!input || !tbody || !totalSpan || !btnConfirmar) return;

        function recalcularTotal() {
            let total = 0;
            tbody.querySelectorAll('tr').forEach(tr => {
                const subtotal = parseFloat(tr.dataset.subtotal || "0");
                total += subtotal;
            });
            totalSpan.textContent = total.toFixed(2);
        }

        input.addEventListener('keydown', function (e) {
            if (e.key !== 'Enter') return;

            e.preventDefault();
            const codigo = this.value.trim();
            if (!codigo) return;

            fetch(`${urlBuscarProducto}?codigo=${encodeURIComponent(codigo)}`)
                .then(r => {
                    if (!r.ok) throw new Error("No encontrado");
                    return r.json();
                })
                .then(prod => {

                    // 1) Si ya existe la fila del producto, incrementa cantidad
                    const filaExistente = tbody.querySelector(`tr[data-id="${prod.id}"]`);
                    if (filaExistente) {
                        const inputCant = filaExistente.querySelector('.input-cantidad');
                        let cant = parseInt(inputCant.value || "1", 10);
                        cant++;
                        inputCant.value = cant;

                        const precio = parseFloat(filaExistente.dataset.precio || "0");
                        const subtotal = precio * cant;

                        filaExistente.dataset.subtotal = subtotal.toString();
                        filaExistente.querySelector('.celda-subtotal').textContent = subtotal.toFixed(2);

                        recalcularTotal();
                        this.value = '';
                        focusScanner();
                        return;
                    }

                    // 2) Crear fila nueva (con input editable)
                    const tr = document.createElement('tr');
                    tr.dataset.id = prod.id;
                    tr.dataset.precio = prod.precio.toString();
                    tr.dataset.subtotal = prod.precio.toString();

                    tr.innerHTML = `
        <td>${prod.nombre}</td>
        <td>
            <input type="number" min="1" value="1"
                   class="form-control form-control-sm input-cantidad"
                   style="width:80px; text-align:center;" />
        </td>
        <td>${prod.precio.toFixed(2)}</td>
        <td class="celda-subtotal">${prod.precio.toFixed(2)}</td>
        <td>
            <button type="button" class="btn btn-sm btn-outline-danger btn-quitar">X</button>
        </td>
    `;

                    // 3) Al cambiar cantidad -> recalcular subtotal y total
                    const inputCant = tr.querySelector('.input-cantidad');
                    inputCant.addEventListener('change', () => {
                        let cant = parseInt(inputCant.value || "1", 10);
                        if (cant < 1) cant = 1;
                        inputCant.value = cant;

                        const precio = parseFloat(tr.dataset.precio || "0");
                        const subtotal = precio * cant;

                        tr.dataset.subtotal = subtotal.toString();
                        tr.querySelector('.celda-subtotal').textContent = subtotal.toFixed(2);

                        inputCant.addEventListener('blur', () => {
                            setTimeout(focusScanner, 0);
                        });

                        recalcularTotal();
                    });

                    // 4) Botón quitar
                    tr.querySelector('.btn-quitar').addEventListener('click', () => {
                        tr.remove();
                        recalcularTotal();
                        focusScanner();
                    });

                    tbody.appendChild(tr);
                    recalcularTotal();
                    this.value = '';
                    focusScanner();
                })

                .catch(err => {
                    console.error(err);
                    mostrarToast("No se encontró el producto", "error");
                    this.value = '';
                    focusScanner();
                });
        });

        btnConfirmar.addEventListener('click', () => {
            const total = parseFloat(totalSpan.textContent) || 0;

            if (total <= 0) {
                mostrarToast("No hay productos en la venta.", "info");
                return;
            }

            fetch(urlConfirmarVenta, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ total: total, formaPago: medioPagoSelect.value })
            })
                .then(r => r.json())
                .then(data => {
                    mostrarToast("Venta registrada N°: " + data.ventaId, "exito");
                    tbody.innerHTML = "";
                    totalSpan.textContent = "0.00";
                })
                .catch(() => {
                    mostrarToast("Error al guardar venta.", "error");
                });
        });

        // ========================================================
        // ÍTEM MANUAL (Modal)
        // ========================================================
        const btnAbrirManual = document.getElementById('btnAbrirManual');
        const btnAgregarManual = document.getElementById('btnAgregarManual');
        const modalManualEl = document.getElementById('modalItemManual');

        const inputDesc = document.getElementById('manualDescripcion');
        const inputCantM = document.getElementById('manualCantidad');
        const inputImp = document.getElementById('manualImporte');
        const labelSub = document.getElementById('manualSubtotal');

        function actualizarSubtotalManual() {
            if (!inputCantM || !inputImp || !labelSub) return;
            const cant = parseInt(inputCantM.value || "1");
            const imp = parseFloat((inputImp.value || "0").replace(",", "."));
            const sub = (cant > 0 ? cant : 1) * (imp >= 0 ? imp : 0);
            labelSub.textContent = "$" + sub.toFixed(2);
        }

        function agregarLineaManual(descripcion, cantidad, importe) {
            const sub = cantidad * importe;

            const tr = document.createElement('tr');
            tr.dataset.manual = "true";
            tr.dataset.precio = importe.toString();
            tr.dataset.subtotal = sub.toString();

            tr.innerHTML = `
        <td>${descripcion || "Ítem manual"}</td>
        <td>
          <input type="number" min="1" value="${cantidad}"
                 class="form-control form-control-sm input-cantidad"
                 style="width:80px;" />
        </td>
        <td>${importe.toFixed(2)}</td>
        <td class="celda-subtotal">${sub.toFixed(2)}</td>
        <td>
          <button type="button" class="btn btn-sm btn-outline-danger btn-quitar">X</button>
        </td>
      `;

            const inputCant = tr.querySelector('.input-cantidad');
            inputCant.addEventListener('change', () => {
                let cant = parseInt(inputCant.value || "1");
                if (cant < 1) cant = 1;
                inputCant.value = cant;

                const precio = parseFloat(tr.dataset.precio);
                const subtotal = precio * cant;
                tr.dataset.subtotal = subtotal;
                tr.querySelector('.celda-subtotal').textContent = subtotal.toFixed(2);

                recalcularTotal();
            });

            tr.querySelector('.btn-quitar').addEventListener('click', () => {
                tr.remove();
                recalcularTotal();
            });

            tbody.appendChild(tr);
            recalcularTotal();
        }

        // Abrir modal
        if (btnAbrirManual && modalManualEl && inputDesc && inputCantM && inputImp && labelSub) {
            btnAbrirManual.addEventListener('click', () => {
                const modal = bootstrap.Modal.getOrCreateInstance(modalManualEl);
                inputDesc.value = "";
                inputCantM.value = "1";
                inputImp.value = "";
                actualizarSubtotalManual();
                modal.show();
                setTimeout(() => inputDesc.focus(), 150);
            });

            [inputCantM, inputImp].forEach(el => {
                el.addEventListener('input', actualizarSubtotalManual);
            });
        }

        // Confirmar agregar ítem manual
        if (btnAgregarManual && modalManualEl && inputDesc && inputCantM && inputImp) {
            btnAgregarManual.addEventListener('click', () => {
                const descripcion = inputDesc.value.trim();
                const cantidad = parseInt(inputCantM.value || "1");
                const importe = parseFloat((inputImp.value || "0").replace(",", "."));

                if (!importe || importe <= 0) {
                    mostrarToast("Ingresá un importe válido.", "error");
                    return;
                }

                agregarLineaManual(descripcion, cantidad > 0 ? cantidad : 1, importe);

                const modal = bootstrap.Modal.getInstance(modalManualEl);
                modal?.hide();

                setTimeout(() => input.focus(), 150);
            });
        }


    })();

});
