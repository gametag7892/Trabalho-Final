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
    private Size tamanhoOriginalForm;
    private List<ControleOriginal> controlesOriginais;

    private readonly Dictionary<int, ProdutoLouco> produtoLoucos = new Dictionary<int, ProdutoLouco>();

    public List<ProdutoLoucoView> MapearProdutosParaView()
    {
        List<ProdutoLoucoView> listaParaExibicao = new List<ProdutoLoucoView>();

        foreach (var par in produtoLoucos)
        {
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

    private void Form1_Load(object sender, EventArgs e)
    {

        tamanhoOriginalForm = this.ClientSize;
        controlesOriginais = new List<ControleOriginal>();

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

        dataGridView1.DataSource = null;
        dataGridView2.DataSource = null;
        dataGridView3.DataSource = null;
        dataGridView4.DataSource = null;

        List<ProdutoLoucoView> listaMapeada = MapearProdutosParaView();

        dataGridView1.DataSource = listaMapeada;
        dataGridView2.DataSource = listaMapeada;
        dataGridView3.DataSource = listaMapeada;
        dataGridView4.DataSource = listaMapeada;

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

        foreach (var par in produtoLoucos)
        {
            if (par.Value.Name.Equals(nomeAntigo, StringComparison.OrdinalIgnoreCase))
            {
                idDoProduto = par.Key;
                produtoParaAtualizar = par.Value;
                break;
            }
        }


        if (idDoProduto != -1)
        {
            if (!string.IsNullOrEmpty(novoNome))
            {
                produtoParaAtualizar.Name = novoNome;
            }

            if (preco.HasValue)
            {
                produtoParaAtualizar.preco = preco.Value;
            }

            if (quantidade.HasValue)
            {
                produtoParaAtualizar.quantidade = quantidade.Value;
            }
        }

        return idDoProduto;
    }

    public int Delete(string nome)
    {
        int idRemovido = -1;
        
        foreach (var par in produtoLoucos)
        {
            if (par.Value.Name.Equals(nome, StringComparison.OrdinalIgnoreCase))
            {
                idRemovido = par.Key;
                break; 
            }
        }
        
        if (idRemovido != -1)
        {
            produtoLoucos.Remove(idRemovido);
        }
        
        return idRemovido;
    }

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

    private void Form1_Resize(object sender, EventArgs e)
    {
        if (this.WindowState == FormWindowState.Minimized)
        {
            return;
        }


        float fatorEscalaX = (float)this.ClientSize.Width / tamanhoOriginalForm.Width;
        float fatorEscalaY = (float)this.ClientSize.Height / tamanhoOriginalForm.Height;

        float fatorEscalaComum = Math.Min(fatorEscalaX, fatorEscalaY);



        foreach (var original in controlesOriginais)
        {
            AjustarControle(original, fatorEscalaComum, fatorEscalaX, fatorEscalaY);
        }
    }


    private void AjustarControle(ControleOriginal original, float fatorEscalaComum, float fatorEscalaX, float fatorEscalaY)
    {
        Control control = original.Controle;

        int novaLargura = (int)(original.Tamanho.Width * fatorEscalaX);
        int novaAltura = (int)(original.Tamanho.Height * fatorEscalaY);
        control.Size = new Size(novaLargura, novaAltura);

        int novaPosicaoX = (int)(original.Posicao.X * fatorEscalaX);
        int novaPosicaoY = (int)(original.Posicao.Y * fatorEscalaY);
        control.Location = new Point(novaPosicaoX, novaPosicaoY);

        float novoTamanhoFonte = original.TamanhoFonte * fatorEscalaComum;

        if (novoTamanhoFonte > 0)
        {
            control.Font = new Font(control.Font.FontFamily, novoTamanhoFonte, control.Font.Style);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {

        string NomeProduto;
        decimal precoProduto;
        int quantidadeProduto;


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

        string nomeBusca = textBox6.Text.Trim();

        string nomeNovo = textBox9.Text.Trim();

        decimal? novoPreco = null;
        int? novaQuantidade = null;

        if (string.IsNullOrEmpty(nomeBusca))
        {
            MessageBox.Show("Por favor, informe o Nome do produto Antigo para realizar a busca.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }


        if (!string.IsNullOrEmpty(textBox8.Text) && decimal.TryParse(textBox8.Text, out decimal precoParsed))
        {
            novoPreco = precoParsed;
        }


        if (!string.IsNullOrEmpty(textBox7.Text) && int.TryParse(textBox7.Text, out int qtdParsed))
        {
            novaQuantidade = qtdParsed;
        }

        if (!novoPreco.HasValue && !novaQuantidade.HasValue)
        {
            MessageBox.Show("Preencha pelo menos um campo (Valor ou Quantidade) para atualizar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int idAtualizado = Update(nomeBusca, nomeNovo, novoPreco, novaQuantidade);

        if (idAtualizado != -1)
        {
            MessageBox.Show($"Produto '{nomeBusca}' atualizado com sucesso! ID: {idAtualizado}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ExibirProdutosNoGrid();
        }
        else
        {
            MessageBox.Show($"Produto com nome '{nomeBusca}' não encontrado no registro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void button4_Click(object sender, EventArgs e)
    {
        string nomeBusca = textBox12.Text.Trim();
        if (string.IsNullOrEmpty(nomeBusca))
        {
            MessageBox.Show("Informe o Nome do produto que deseja deletar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            textBox12.Focus();
            return;
        }
        
        int idDeletado = Delete(nomeBusca);

        if (idDeletado != -1)
        {
            MessageBox.Show($"Produto '{nomeBusca}' (ID: {idDeletado}) deletado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ExibirProdutosNoGrid();

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
    public int ID { get; set; }

    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}
