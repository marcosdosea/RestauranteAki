
document.getElementById('cep-input').addEventListener('blur', function () {
    const cep = this.value.replace(/\D/g, ''); 

    if (cep.length === 8) {
        fetch(`/api/RestauranteApi/consultar-cep/${cep}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('CEP não encontrado');
                }
                return response.json();
            })
            .then(data => {
                document.getElementById('rua-input').value = data.logradouro;
                document.getElementById('bairro-input').value = data.bairro;
                document.getElementById('cidade-input').value = data.localidade;
                document.getElementById('estado-input').value = data.uf;
            })
            .catch(error => {
                console.error('Erro ao buscar CEP:', error);
            });
    }
});