document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('ingredientesModal');
    if (!modalEl) return;

    const apiUrl = modalEl.dataset.apiUrl;
    const listaModalUl = document.getElementById('lista-modal');
    const novoIngredienteInput = document.getElementById('novo-ingrediente-modal');
    const btnAdicionar = document.getElementById('btn-adicionar-modal');
    const btnSalvar = document.getElementById('btn-salvar-modal');
    const descricaoInput = document.getElementById('descricao');

    const validationSpan = document.querySelector('span[data-valmsg-for="Descricao"]');

    function showValidationMessage(msg) {
        if (validationSpan) {
            validationSpan.textContent = msg;
            validationSpan.classList.remove('field-validation-valid');
            validationSpan.classList.add('field-validation-error');
        } else {
            console.warn('Validation span for Descricao not found:', msg);
        }
    }

    function clearValidationMessage() {
        if (validationSpan) {
            validationSpan.textContent = '';
            validationSpan.classList.remove('field-validation-error');
            validationSpan.classList.add('field-validation-valid');
        }
    }

    function criarItemHtml(nome, isChecked) {
        const idSeguro = `ing-${nome.replace(/[^a-zA-Z0-9]/g, '-')}`;
        return `
            <li class="list-group-item d-flex justify-content-between align-items-center">
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="${nome}" id="${idSeguro}" ${isChecked ? 'checked' : ''}>
                    <label class="form-check-label" for="${idSeguro}">${nome}</label>
                </div>
                <div>
                    <button type="button" class="btn btn-sm btn-outline-primary btn-editar me-1" title="Editar nome">✏️</button>
                    <button type="button" class="btn btn-sm btn-outline-danger btn-remover" title="Remover item">🗑️</button>
                </div>
            </li>`;
    }

    function renderizarLista(ingredientesDisponiveis, ingredientesJaSelecionados) {
        listaModalUl.innerHTML = '';
        const todosOsIngredientes = [...new Set([...ingredientesDisponiveis, ...ingredientesJaSelecionados])].sort();
        todosOsIngredientes.forEach(nome => {
            if (!nome) return;
            const isChecked = ingredientesJaSelecionados.some(sel => sel.toLowerCase() === nome.toLowerCase());
            listaModalUl.insertAdjacentHTML('beforeend', criarItemHtml(nome, isChecked));
        });
    }

    function adicionarNovoItemNaLista(nome) {
        const nomeNormalizado = nome.toLowerCase();
        for (let input of listaModalUl.querySelectorAll('input[type="checkbox"]')) {
            if (input.value.toLowerCase() === nomeNormalizado) {
                input.checked = true;
                clearValidationMessage();
                return;
            }
        }
        listaModalUl.insertAdjacentHTML('beforeend', criarItemHtml(nome, true));
        clearValidationMessage();
    }

    listaModalUl.addEventListener('click', (e) => {
        const itemLi = e.target.closest('.list-group-item');
        if (!itemLi) return;

        if (e.target.closest('.btn-remover')) {
            itemLi.remove();
            return;
        }

        if (e.target.closest('.btn-editar')) {
            const checkbox = itemLi.querySelector('.form-check-input');
            const label = itemLi.querySelector('.form-check-label');
            const nomeAntigo = checkbox.value;
            const novoNome = prompt("Editar ingrediente:", nomeAntigo);
            if (novoNome && novoNome.trim() !== "" && novoNome.trim() !== nomeAntigo) {
                const nomeFinal = novoNome.trim();
                checkbox.value = nomeFinal;
                label.textContent = nomeFinal;
            }
        }
    });

    modalEl.addEventListener('show.bs.modal', async () => {
        try {
            const response = await fetch(apiUrl);
            if (!response.ok) throw new Error(`Falha na API: ${response.status}`);

            const ingredientesDisponiveis = await response.json();
            const ingredientesJaSelecionados = descricaoInput.value ? descricaoInput.value.split(',').map(i => i.trim()).filter(Boolean) : [];
            renderizarLista(ingredientesDisponiveis, ingredientesJaSelecionados);

            if (ingredientesJaSelecionados.length > 0) clearValidationMessage();
        } catch (error) {
            console.error('Erro ao carregar ingredientes:', error);
            listaModalUl.innerHTML = `<li class="list-group-item text-danger"><b>Erro:</b> Não foi possível carregar os ingredientes.</li>`;
        }
    });

    btnAdicionar.addEventListener('click', () => {
        const nome = novoIngredienteInput.value.trim();
        if (nome) {
            adicionarNovoItemNaLista(nome);
            novoIngredienteInput.value = "";
            novoIngredienteInput.focus();
        }
    });

    novoIngredienteInput.addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            e.preventDefault();
            btnAdicionar.click();
        }
    });

    listaModalUl.addEventListener('change', (e) => {
        if (e.target && e.target.matches('input[type="checkbox"]')) {
            const anyChecked = listaModalUl.querySelector('input[type="checkbox"]:checked') !== null;
            if (anyChecked) clearValidationMessage();
            else showValidationMessage("É necessário informar ao menos 1 ingrediente.");
        }
    });

    btnSalvar.addEventListener('click', () => {
        const checkboxesMarcados = listaModalUl.querySelectorAll('input[type="checkbox"]:checked');
        const nomesSelecionados = Array.from(checkboxesMarcados).map(cb => cb.value.trim()).filter(Boolean);
        descricaoInput.value = nomesSelecionados.join(',');

        if (nomesSelecionados.length === 0) {
            showValidationMessage("É necessário informar ao menos 1 ingrediente.");
            novoIngredienteInput && novoIngredienteInput.focus();
            return;
        }

        clearValidationMessage();
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();
    });
});
