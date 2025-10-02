document.addEventListener('DOMContentLoaded', function () {
    // Pega a referência para o modal do Bootstrap
    const cepModal = new bootstrap.Modal(document.getElementById('cepModal'));
    const btnAbrirModal = document.getElementById('btn-abrir-modal-cep');
    const btnPreencher = document.getElementById('btn-preencher-endereco');
    const cepInputModal = document.getElementById('cep-modal-input');
    const errorDiv = document.getElementById('cep-modal-error');

    if (btnAbrirModal) {
        btnAbrirModal.addEventListener('click', function () {
            cepInputModal.value = ''; 
            errorDiv.textContent = ''; 
            cepModal.show();
        });
    }
    if (btnPreencher) {
        btnPreencher.addEventListener('click', function () {
            const cep = cepInputModal.value.replace(/\D/g, '');
            errorDiv.textContent = ''; 

            if (cep.length === 8) {
                fetch(`/api/RestauranteApi/consultar-cep/${cep}`)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('CEP não encontrado ou inválido.');
                        }
                        return response.json();
                    })
                    .then(data => {
                        document.getElementById('cep-input').value = data.cep;
                        document.getElementById('rua-input').value = data.logradouro;
                        document.getElementById('bairro-input').value = data.bairro;
                        document.getElementById('cidade-input').value = data.localidade;
                        document.getElementById('estado-input').value = data.uf;

                        cepModal.hide(); 
                    })
                    .catch(error => {
                        errorDiv.textContent = error.message; 
                    });
            } else {
                errorDiv.textContent = 'Por favor, digite um CEP válido com 8 dígitos.';
            }
        });
    }
});