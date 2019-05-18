using com.puc.dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace com.puc.dashboard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<LogDns> requisicoes = new List<LogDns>();

            try { 
                var client = new RestClient("https://dnsproxy.azurewebsites.net");
                var requisicao = new RestRequest("api/Log", Method.GET);
                IRestResponse resposta = client.Execute(requisicao);
                requisicoes = JsonConvert.DeserializeObject<List<LogDns>>(resposta.Content);
            }
            catch
            { }

            ViewBag.Requisicoes = requisicoes;
            ViewBag.ContadorOrigemHora = ContadorOrigensUltimaHora(requisicoes);
            ViewBag.ContadorDestinoHora = ContadorDestinoUltimaHora(requisicoes);
            ViewBag.ContadorOrigem24 = ContadorOrigensUltimas24Horas(requisicoes);
            ViewBag.ContadorDestino24 = ContadorDestinoUltimas24Horas(requisicoes);
            ViewBag.OrigemGrafico = ContadorOrigensGrafico(requisicoes);
            ViewBag.DestinoGrafico = ContadorDestinoGrafico(requisicoes);
            ViewBag.DataHora = DateTime.Now;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private long ContadorOrigensUltimaHora(List<LogDns> requisicoes)
        {
            if (requisicoes != null && requisicoes.Count > 0)
            {
                return requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-1)).Select(x => x.Origem).Distinct().Count();
            }
            else
                return -1;
        }

        private long ContadorDestinoUltimaHora(List<LogDns> requisicoes)
        {
            if (requisicoes != null && requisicoes.Count > 0)
                return requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-1)).Select(x => x.Destino).Distinct().Count();
            else
                return -1;
        }

        private long ContadorOrigensUltimas24Horas(List<LogDns> requisicoes)
        {
            if (requisicoes != null && requisicoes.Count > 0)
                return requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-24)).Select(x => x.Origem).Distinct().Count();
            else
                return -1;
        }

        private long ContadorDestinoUltimas24Horas(List<LogDns> requisicoes)
        {
            if (requisicoes != null && requisicoes.Count > 0)
                return requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-24)).Select(x => x.Destino).Distinct().Count();
            else
                return -1;
        }

        private List<long> ContadorOrigensGrafico(List<LogDns> requisicoes)
        {
            List<long> dados = new List<long>();

            if (requisicoes != null && requisicoes.Count > 0) { 
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-24)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-21)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-18)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-15)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-12)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-9)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-6)).Select(x => x.Origem).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-3)).Select(x => x.Origem).Distinct().Count());
            }

            return dados;
        }

        private List<long> ContadorDestinoGrafico(List<LogDns> requisicoes)
        {
            List<long> dados = new List<long>();

            if (requisicoes != null && requisicoes.Count > 0)
            {
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-24)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-21)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-18)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-15)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-12)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-9)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-6)).Select(x => x.Destino).Distinct().Count());
                dados.Add(requisicoes.Where(c => c.DataHora > DateTime.Now.AddHours(-3)).Select(x => x.Destino).Distinct().Count());
            }

            return dados;
        }
    }
}
