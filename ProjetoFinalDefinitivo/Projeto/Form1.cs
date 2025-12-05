using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Projeto;
public struct ControleOriginal
{
    public Control Controle;
    public Size Tamanho;
    public Point Posicao;
    public float TamanhoFonte;
}

public partial class Form1 : Form
{

    // Variáveis para armazenar o estado original dos elementos
    private Size tamanhoOriginalForm;
    private List<ControleOriginal> controlesOriginais;

    private readonly Dictionary<int, ProdutoLouco> produtoLoucos = new Dictionary<int, ProdutoLouco>();

    public List<ProdutoLoucoView> MapearProdutosParaView()
    {
        List<ProdutoLoucoView> listaParaExibicao = new List<ProdutoLoucoView>();

        foreach (var par in produtoLoucos)
        {
            // 'par.Key' é o ID
            // 'par.Value' é o objeto ProdutoLouco
            listaParaExibicao.Add(new ProdutoLoucoView
            {
                ID = par.Key,
                Nome = par.Value.Name,
                Preco = par.Value.preco,
                Quantidade = par.Value.quantidade
            });
        }

        return listaParaExibicao;
    }



    public Form1()
    {
        InitializeComponent();
        this.Resize += new EventHandler(Form1_Resize);
    }

    // --- 1. Evento LOAD: Captura dos Valores Iniciais ---
    private void Form1_Load(object sender, EventArgs e)
    {
        // 1. Capturar o tamanho original do formulário
        tamanhoOriginalForm = this.ClientSize;

        // 2. Inicializar a lista
        controlesOriginais = new List<ControleOriginal>();

        // 3. Adicionar cada Label e Input à lista, registrando seus valores originais
        // ATENÇÃO: Verifique se estes nomes correspondem aos nomes dos seus controles no designer.

        // Labels
        AdicionarControleOriginal(label1);
        AdicionarControleOriginal(label2);
        AdicionarControleOriginal(label3);
        AdicionarControleOriginal(label4);
        AdicionarControleOriginal(label5);
        AdicionarControleOriginal(label6);
        AdicionarControleOriginal(label7);
        AdicionarControleOriginal(label8);
        AdicionarControleOriginal(label9);
        AdicionarControleOriginal(label12);

        // TextBoxes (Inputs)
        AdicionarControleOriginal(textBox1);
        AdicionarControleOriginal(textBox2);
        AdicionarControleOriginal(textBox3);
        AdicionarControleOriginal(textBox4);
        AdicionarControleOriginal(textBox5);
        AdicionarControleOriginal(textBox6);
        AdicionarControleOriginal(textBox7);
        AdicionarControleOriginal(textBox8);
        AdicionarControleOriginal(textBox9);
        AdicionarControleOriginal(textBox12);

        // Fotos
        AdicionarControleOriginal(pictureBox1);
        AdicionarControleOriginal(pictureBox2);

        // DataGrids
        AdicionarControleOriginal(dataGridView1);
        AdicionarControleOriginal(dataGridView2);
        AdicionarControleOriginal(dataGridView3);
        AdicionarControleOriginal(dataGridView4);

        // Botões
        AdicionarControleOriginal(button1);
        AdicionarControleOriginal(button3);
        AdicionarControleOriginal(button4);
    }

    private void ExibirProdutosNoGrid()
    {
        // 1. Limpa a fonte de dados anterior (essencial)
        dataGridView1.DataSource = null;
        dataGridView2.DataSource = null;
        dataGridView3.DataSource = null;
        dataGridView4.DataSource = null;

        // 2. Chama a função de mapeamento
        List<ProdutoLoucoView> listaMapeada = MapearProdutosParaView();

        // 3. Define a lista como a nova fonte de dados
        dataGridView1.DataSource = listaMapeada;
        dataGridView2.DataSource = listaMapeada;
        dataGridView3.DataSource = listaMapeada;
        dataGridView4.DataSource = listaMapeada;

        // O DataGridView criará automaticamente colunas para cada propriedade (ID, Nome, Preco, Quantidade)
    }

    private int Id = 1;

    private int Create(string nome, decimal preco, int quantidade)
    {
        int id = Id;

        produtoLoucos.Add(id, new ProdutoLouco
        {
            Name = nome,
            preco = preco,
            quantidade = quantidade,
        });

        Id++;


        ExibirProdutosNoGrid();
        return id;
    }

    public int Update(string nomeAntigo, string novoNome, decimal? preco, int? quantidade)
    {
        int idDoProduto = -1;
        ProdutoLouco produtoParaAtualizar = null;

        // 1. BUSCAR o produto no dicionário pelo nome antigo
        foreach (var par in produtoLoucos)
        {
            if (par.Value.Name.Equals(nomeAntigo, StringComparison.OrdinalIgnoreCase))
            {
                idDoProduto = par.Key;
                produtoParaAtualizar = par.Value;
                break;
            }
        }

        // 2. Aplicar as atualizações se o produto foi encontrado
        if (idDoProduto != -1)
        {
            // --- LÓGICA PARA ALTERAR O NOME ---
            // Verifica se um novo nome foi fornecido e não está vazio
            if (!string.IsNullOrEmpty(novoNome))
            {
                // Opcional: Adicionar validação para garantir que o novo nome não existe
                // Antes de alterar o nome, você pode querer verificar se já existe um produto com esse novo nome.

                produtoParaAtualizar.Name = novoNome;
            }

            // --- LÓGICA PARA ALTERAR PREÇO E QUANTIDADE ---
            if (preco.HasValue)
            {
                produtoParaAtualizar.preco = preco.Value;
            }

            if (quantidade.HasValue)
            {
                produtoParaAtualizar.quantidade = quantidade.Value;
            }

            // Se estiver usando DataGridView:
            // ExibirProdutosNoGrid(); 
        }

        return idDoProduto;
    }

    public int Delete(string nome)
    {
        int idRemovido = -1;

        // 1. Encontrar o ID (Chave) associado ao Nome
        // É necessário iterar, pois a chave não é o nome, e sim o ID.
        foreach (var par in produtoLoucos)
        {
            if (par.Value.Name.Equals(nome, StringComparison.OrdinalIgnoreCase))
            {
                idRemovido = par.Key;
                break; // Encontrou, pode sair do loop
            }
        }

        // 2. Se o ID foi encontrado, remove o item usando a chave
        if (idRemovido != -1)
        {
            produtoLoucos.Remove(idRemovido);
        }

        // Retorna o ID do produto removido, ou -1 se não foi encontrado
        return idRemovido;
    }

    // Função auxiliar para simplificar a adição de um controle
    private void AdicionarControleOriginal(Control control)
    {
        controlesOriginais.Add(new ControleOriginal
        {
            Controle = control,
            Tamanho = control.Size,
            Posicao = control.Location,
            TamanhoFonte = control.Font.Size
        });
    }

    // --- 2. Evento RESIZE: Cálculo e Aplicação Proporcional ---
    private void Form1_Resize(object sender, EventArgs e)
    {
        // Se a janela estiver minimizada, ignora para evitar erros de cálculo
        if (this.WindowState == FormWindowState.Minimized)
        {
            return;
        }

        // 1. Calcula os fatores de escala
        float fatorEscalaX = (float)this.ClientSize.Width / tamanhoOriginalForm.Width;
        float fatorEscalaY = (float)this.ClientSize.Height / tamanhoOriginalForm.Height;

        // Fator de escala comum (o menor) para manter a proporção da fonte
        float fatorEscalaComum = Math.Min(fatorEscalaX, fatorEscalaY);


        // 2. Itera sobre todos os controles registrados e os ajusta
        foreach (var original in controlesOriginais)
        {
            AjustarControle(original, fatorEscalaComum, fatorEscalaX, fatorEscalaY);
        }
    }

    // --- 3. Função de AJUSTE: Lógica de Redimensionamento ---
    private void AjustarControle(ControleOriginal original, float fatorEscalaComum, float fatorEscalaX, float fatorEscalaY)
    {
        Control control = original.Controle;

        // 1. Redimensionar o Controle (Largura e Altura)
        // Usamos o fator de escala de cada eixo (X e Y) para preenchimento
        int novaLargura = (int)(original.Tamanho.Width * fatorEscalaX);
        int novaAltura = (int)(original.Tamanho.Height * fatorEscalaY);
        control.Size = new Size(novaLargura, novaAltura);

        // 2. Reposicionar o Controle (Location)
        int novaPosicaoX = (int)(original.Posicao.X * fatorEscalaX);
        int novaPosicaoY = (int)(original.Posicao.Y * fatorEscalaY);
        control.Location = new Point(novaPosicaoX, novaPosicaoY);

        // 3. Redimensionar a Fonte
        // Usa o fator de escala comum para manter a proporção da fonte
        float novoTamanhoFonte = original.TamanhoFonte * fatorEscalaComum;

        // Evita tamanhos de fonte inválidos e cria um novo objeto Font
        if (novoTamanhoFonte > 0)
        {
            control.Font = new Font(control.Font.FontFamily, novoTamanhoFonte, control.Font.Style);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        // 1. Declare uma variável do tipo string
        string NomeProduto;
        decimal precoProduto;
        int quantidadeProduto;

        // 2. Acesse a propriedade Text do TextBox
        NomeProduto = textBox2.Text;
        precoProduto = decimal.Parse(textBox3.Text);
        quantidadeProduto = int.Parse(textBox1.Text);

        Create(NomeProduto, precoProduto, quantidadeProduto);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        ExibirProdutosNoGrid();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        // 1. CAPTURAR E VALIDAR OS DADOS

        // Campo de identificação (o nome que será buscado)
        string nomeBusca = textBox6.Text.Trim();

        string nomeNovo = textBox9.Text.Trim();

        // Campos de atualização (podem ser opcionais/null)
        decimal? novoPreco = null;
        int? novaQuantidade = null;

        // Verifica se o campo de busca está vazio
        if (string.IsNullOrEmpty(nomeBusca))
        {
            MessageBox.Show("Por favor, informe o Nome do produto Antigo para realizar a busca.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Tenta converter o novo preço (se o campo não estiver vazio)
        if (!string.IsNullOrEmpty(textBox8.Text) && decimal.TryParse(textBox8.Text, out decimal precoParsed))
        {
            novoPreco = precoParsed;
        }

        // Tenta converter a nova quantidade (se o campo não estiver vazio)
        if (!string.IsNullOrEmpty(textBox7.Text) && int.TryParse(textBox7.Text, out int qtdParsed))
        {
            novaQuantidade = qtdParsed;
        }

        // Verifica se pelo menos um campo de atualização foi preenchido
        if (!novoPreco.HasValue && !novaQuantidade.HasValue)
        {
            MessageBox.Show("Preencha pelo menos um campo (Valor ou Quantidade) para atualizar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }


        // 2. CHAMAR O MÉTODO UPDATE

        // O método Update que definimos retorna o ID atualizado ou -1 se não encontrar.
        int idAtualizado = Update(nomeBusca, nomeNovo, novoPreco, novaQuantidade);


        // 3. TRATAMENTO DO RESULTADO
        if (idAtualizado != -1)
        {
            MessageBox.Show($"Produto '{nomeBusca}' atualizado com sucesso! ID: {idAtualizado}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 4. ATUALIZAR O GRID (Se o seu DataGridView se chama 'dadosGrid')
            ExibirProdutosNoGrid();

            // Opcional: Limpar os campos após o sucesso
            // txtNomeAntigo.Clear();
            // txtValorNovo.Clear();
            // txtQtdNova.Clear();
        }
        else
        {
            MessageBox.Show($"Produto com nome '{nomeBusca}' não encontrado no registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button4_Click(object sender, EventArgs e)
    {
        // Substitua 'txtNomeDeletar' pelo nome real do seu TextBox de busca
        string nomeBusca = textBox12.Text.Trim();

        // 1. Validação
        if (string.IsNullOrEmpty(nomeBusca))
        {
            MessageBox.Show("Informe o Nome do produto que deseja deletar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox12.Focus();
            return;
        }

        // 2. Chama a função de deleção
        int idDeletado = Delete(nomeBusca);

        // 3. Tratamento do Resultado
        if (idDeletado != -1)
        {
            MessageBox.Show($"Produto '{nomeBusca}' (ID: {idDeletado}) deletado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 4. ATUALIZA O GRID
            ExibirProdutosNoGrid();

            // Limpa o campo de busca
            textBox12.Clear();
        }
        else
        {
            MessageBox.Show($"Produto com nome '{nomeBusca}' não encontrado no registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}

public class ProdutoLouco
{
    public string Name { get; set; }
    public decimal preco { get; set; }
    public int quantidade { get; set; }
}

public class ProdutoLoucoView
{
    // Adicione o campo ID que você gerou
    public int ID { get; set; }

    // Mantenha os campos originais
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}
