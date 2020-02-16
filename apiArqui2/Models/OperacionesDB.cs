using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using apiArqui2;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class DBManager
{
    MySqlConnection mysqlcon;
    String conexionStringMysql;

    public DBManager()
    {
        conexionStringMysql = "server=mysql;port=3306;Database=arqui2;user=root;Password=Abc123**;";
    }


    /// <summary>
    /// Clases principales para aperturar y cerrar la conexion para la conexion MySQL 
    /// </summary>
    #region CONEXIONES MYSQL

    private void openConexionMySQL()
    {
        try
        {
            mysqlcon = new MySqlConnection(conexionStringMysql);
            if (mysqlcon.State == ConnectionState.Closed)
                mysqlcon.Open();
        }
        catch (Exception ex) {
            Console.Write(ex.Message);
        }
    }

    public String status()
    {
        try
        {
            mysqlcon = new MySqlConnection(conexionStringMysql);
            if (mysqlcon.State == ConnectionState.Closed)
                mysqlcon.Open();

            mysqlcon.Close();
            return "true";

        }
        catch (Exception ex) {
            Console.Write(ex.Message);
            return "false";
        }
    }

    private void closeConexionMySQL()
    {
        try
        {
            if (mysqlcon == null) mysqlcon = new MySqlConnection(conexionStringMysql);
            if (mysqlcon.State == ConnectionState.Open)
                mysqlcon.Close();
        }
        catch (Exception ex) {
            Console.Write(ex.Message);
        }
    }

    #endregion


    #region METODOS PARA ADMINISTRACION DE BASE DE DATOS

    /// <summary>
    /// Autor: Haroldo Arias
    /// Descripcion: Función que permite ejecutar un query dentro de la base de datos.
    /// </summary>
    /// <param name="query">
    /// Variable String que recibe el query a ejecutar.
    /// </param>
    /// <param name="db_base">
    /// Nombre de la base de datos en donde se correra el query
    /// </param>
    /// <returns> 
    ///     Retornará true si el query fue ejecutado o false si fuera lo contrario.
    /// </returns>
    public bool execQuery(string query, string db_base)
    {
        try
        {
            
                openConexionMySQL();
                mysqlcon.ChangeDatabase(db_base);
                MySqlCommand com = new MySqlCommand();
                com.Connection = mysqlcon;
                com.CommandType = CommandType.Text;
                com.CommandText = query;
                int resultado = com.ExecuteNonQuery();
                closeConexionMySQL();
                if (resultado > 0) return true;
                return false;
            

        }
        catch (Exception ex)
        {
            closeConexionMySQL();
            Console.Write(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Autor: Haroldo Arias
    /// Descripcion: Función que permite obtener un DataTable mediante un query dentro de la base de datos.
    /// </summary>
    /// <param name="query">
    /// Variable String que recibe el query a ejecutar.
    /// </param>
    /// <param name="db_base">
    /// Nombre de la base de datos en donde se correra el query
    /// </param> 
    /// <returns>
    ///     Retornará un DataTable si el la ejecución del query encuentra datos o no tiene errores o retornará null si fuera lo contrario.
    /// </returns>
    public DataTable getTableByQuery(String query, String db_base = "")
    {
        try
        {
            DataTable dataTable = new DataTable();
            
                openConexionMySQL();
                mysqlcon.ChangeDatabase(db_base);
                MySqlCommand com = new MySqlCommand();
                com.Connection = mysqlcon;
                com.CommandType = CommandType.Text;
                com.CommandText = query;
                MySqlDataReader reader = com.ExecuteReader();
                dataTable.Load(reader);
                reader.Close();
                reader.Dispose();
                closeConexionMySQL();
                return dataTable;
            
        }
        catch (Exception ex)
        {
           
                closeConexionMySQL();
           
            return null;
        }
    }

    /// <summary>
    /// Autor: Haroldo Arias
    /// Descripcion: Función que permite obtener un Valor mediante un query dentro de la base de datos, por ejemplo un "Select Nombre From Hospital".
    /// </summary>
    /// <param name="query">
    /// Variable String que recibe el query a ejecutar.
    /// </param>
    /// <param name="db_base">
    /// Nombre de la base de datos en donde se correra el query
    /// </param> 
    /// <returns>
    ///     Retornará un valor no parseado del valor que se haga la consulta si se encuentran datos o no se tuvieron errores, caso contrario se retonará el valor null.
    /// </returns>
    public String getSimpleSelect(string query, string db_base)
    {
        DataTable dato = getTableByQuery(query, db_base);
        if (dato.Rows.Count > 0)
        {
            foreach (DataRow row in dato.Rows)
            {
                return row[0].ToString();
            }
        }
        return "";
    }

    #endregion

}

