﻿using System.Globalization;

CultureInfo culturaBR = new CultureInfo("pt-BR");

string[,] produtos = new string[,] {
    {"7891024110348", "2,88", "2,51", "12"},
    {"7891048038017", "4,40", "4,37", "3"},
    {"7896066334509", "5,19", "---", "---"},
    {"7891700203142", "2,39", "2,38", "6"},
    {"7894321711263", "9,79", "---", "---"},
    {"7896001250611", "9,89", "9,10", "10"},
    {"7793306013029", "12,79", "12,35", "3"},
    {"7896004400914", "4,20", "4,05", "6"},
    {"7898080640017", "6,99", "6,89", "12"},
    {"7891025301516", "12,99", "---", "---"},
    {"7891030003115", "3,12", "3,09", "4"}
};

string[,] pedidos = new string[,] {
    {"7891048038017", "1", "4,40"},
    {"7896004400914", "4", "16,80"},
    {"7891030003115", "1", "3,12"},
    {"7891024110348", "6", "17,28"},
    {"7898080640017", "24", "167,76"},
    {"7896004400914", "8", "33,60"},
    {"7891700203142", "8", "19,12"},
    {"7891048038017", "1", "4,40"},
    {"7793306013029", "3", "38,37"},
    {"7896066334509", "2", "10,38"}
};

int totalProdutos = produtos.GetLength(0);
int totalPedidos = pedidos.GetLength(0);

string[] codigos = new string[totalPedidos];
int[] quantidades = new int[totalPedidos];
int produtosUnicos = 0;


for (int i = 0; i < totalPedidos; i++) {
    string codigo = pedidos[i, 0];
    int quantidade = int.Parse(pedidos[i, 1]);

    int idx = Array.IndexOf(codigos, codigo, 0, produtosUnicos);
    if (idx == -1) {
        codigos[produtosUnicos] = codigo;
        quantidades[produtosUnicos] = quantidade;
        produtosUnicos++;
    } else {
        quantidades[idx] += quantidade;
    }
}

double precoTotal = 0;
double descontoTotal = 0;
double[] descontos = new double[produtosUnicos];

for (int i = 0; i < produtosUnicos; i++) {
    string codigo = codigos[i];
    int quantidade = quantidades[i];

    double precoUnitario = 0;
    double precoAtacado = 0;
    int minAtacado = 0;
    bool possuiAtacado = false;

    for (int j = 0; j < totalProdutos; j++) {
        if (produtos[j, 0] == codigo) {
            precoUnitario = double.Parse(produtos[j, 1], culturaBR);
            if (produtos[j, 2] != "---" && produtos[j, 3] != "---") {
                precoAtacado = double.Parse(produtos[j, 2], culturaBR);
                minAtacado = int.Parse(produtos[j, 3]);
                possuiAtacado = true;
            }
            break;
        }
    }

    precoTotal += precoUnitario * quantidade;

    if (possuiAtacado && quantidade >= minAtacado) {
        double valorNormal = precoUnitario * quantidade;
        double valorComDesconto = precoAtacado * quantidade;
        double desconto = valorNormal - valorComDesconto;

        descontos[i] = desconto;
        descontoTotal += desconto;
    } else {
        descontos[i] = 0;
    }
}

Console.Clear();
Console.WriteLine("------ Desconto no Atacado ------- ");
for (int i = 0; i < produtosUnicos; i++) {
    if (descontos[i] > 0)
        Console.WriteLine($"| {codigos[i],-17} | R$ {descontos[i],7:F2} |");
}

Console.WriteLine($"\n(+) Subtotal = R$ {precoTotal:F2}");
Console.WriteLine($"(-) Descontos aplicados = R$ {descontoTotal:F2}");
Console.WriteLine($"(=) Total = R$ {precoTotal - descontoTotal:F2}");

