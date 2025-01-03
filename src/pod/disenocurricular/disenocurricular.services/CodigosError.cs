﻿namespace disenocurricular.services;

public class CodigosError
{
    #region Códigos Error de Servicios
    //CODIGO GENERALES - ERROR
    public const string DISENOCURRICULAR_ERROR_DESCONOCIDO = "DISENOCURRICULAR-ERROR-DESCONOCIDO";
    public const string DISENOCURRICULAR_DATOS_NO_VALIDOS = "DISENOCURRICULAR-DATOS-NO-VALIDOS";
    public const string DISENOCURRICULAR_SIN_INTERPRETE_CONSULTA = "DISENOCURRICULAR-SIN_INTERPRETE_CONSULTA";
    public const string DISENOCURRICULAR_NO_HAY_INTERPRETE_CONSULTA = "DISENOCURRICULAR-NO-HAY-INTERPRETE-CONSULTA";

    //CODIGO ERROR - CURSO
    public const string DISENOCURRICULAR_CURSO_NO_ENCONTRADA = "DISENOCURRICULAR-CURSO-NO-ENCONTRADA";
    public const string DISENOCURRICULAR_CURSO_ID_PAYLOAD_NO_INGRESADO = "DISENOCURRICULAR-CURSO-ID-PAYLOAD-NO-INGRESADO";
    public const string DISENOCURRICULAR_CURSO_ID_NO_INGRESADO = "DISENOCURRICULAR-CURSO-ID-NO-INGRESADO";
    public const string DISENOCURRICULAR_CURSO_ERROR_ACTUALIZAR = "DISENOCURRICULAR-CURSO-ERROR-ACTUALIZAR";


    //codigo error - Plan
    public const string DISENOCURRICULAR_PLAN_NO_ENCONTRADA = "DISENOCURRICULAR-PLAN-NO-ENCONTRADA";
    public const string DISENOCURRICULAR_PLAN_ID_PAYLOAD_NO_INGRESADO = "DISENOCURRICULAR-PLAN-ID-PAYLOAD-NO-INGRESADO";
    public const string DISENOCURRICULAR_PLAN_ID_NO_INGRESADO = "DISENOCURRICULAR-PLAN-ID-NO-INGRESADO";

    public const string DISENOCURRICULAR_PLAN_ERROR_VALIDACION_CURSO_NOENCONTRADO = "DISENOCURRICULAR-PLAN-ERROR-VALIDACION-CURSO-NOENCONTRADO";

    //CODIGO error - Temario
    public const string DISENOCURRICULAR_TEMARIO_NO_ENCONTRADA = "DISENOCURRICULAR-TEMARIO-NO-ENCONTRADA";
    public const string DISENOCURRICULAR_TEMARIO_ID_PAYLOAD_NO_INGRESADO = "DISENOCURRICULAR-TEMARIO-ID-PAYLOAD-NO-INGRESADO";
    public const string DISENOCURRICULAR_TEMARIO_ID_NO_INGRESADO = "DISENOCURRICULAR-TEMARIO-ID-NO-INGRESADO";

    //CODIGO error - Especialidad
    public const string DISENOCURRICULAR_ESPECIALIDAD_NO_ENCONTRADA = "DISENOCURRICULAR-ESPECIALIDAD-NO-ENCONTRADA";
    public const string DISENOCURRICULAR_ESPECIALIDAD_ID_PAYLOAD_NO_INGRESADO = "DISENOCURRICULAR-ESPECIALIDAD-ID-PAYLOAD-NO-INGRESADO";
    public const string DISENOCURRICULAR_ESPECIALIDAD_ID_NO_INGRESADO = "DISENOCURRICULAR-ESPECIALIDAD-ID-NO-INGRESADO";
    public const string DISENOCURRICULAR_ESPECIALIDAD_ERROR_VALIDACION_CURSO_NOENCONTRADO = "DISENOCURRICULAR-ESPECIALIDAD-ERROR-VALIDACION-CURSO-NOENCONTRADO";

    //CODIGO error - Tema
    public const string DISENOCURRICULAR_TEMA_NO_ENCONTRADA = "DISENOCURRICULAR-TEMA-NO-ENCONTRADA";
    public const string DISENOCURRICULAR_TEMA_ID_PAYLOAD_NO_INGRESADO = "DISENOCURRICULAR-TEMA-ID-PAYLOAD-NO-INGRESADO";
    public const string DISENOCURRICULAR_TEMA_ID_NO_INGRESADO = "DISENOCURRICULAR-TEMA-ID-NO-INGRESADO";
    public const string DISENOCURRICULAR_TEMA_ERROR_ACTUALIZAR = "DISENOCURRICULAR-TEMA-ERROR-ACTUALIZAR";
    public const string DISENOCURRICULAR_TEMA_ERROR_ELIMINAR = "DISENOCURRICULAR-TEMA-ERROR-ELIMINAR";
    public const string DISENOCURRICULAR_TEMA_TEMAID_NO_EXISTE = "DISENOCURRICULAR-TEMA-TEMAID_NO_EXISTE";
    public const string DISENOCURRICULAR_TEMA_EXISTE_TEMAS_DEPENDIENTES = "DISENOCURRICULAR-TEMA-EXISTEN-TEMAS-DEPENDIENTES";

    //CODIGO error - Periodo
    public const string DISENOCURRICULAR_PERIODO_NO_ENCONTRADA = "DISENOCURRICULAR-PERIODO-NO-ENCONTRADA";
    public const string DISENOCURRICULAR_PERIODO_ID_PAYLOAD_NO_INGRESADO = "DISENOCURRICULAR-PERIODO-ID-PAYLOAD-NO-INGRESADO";
    public const string DISENOCURRICULAR_PERIODO_ID_NO_INGRESADO = "DISENOCURRICULAR-PERIODO-ID-NO-INGRESADO";
    public const string DISENOCURRICULAR_PERIODO_ERROR_ACTUALIZAR = "DISENOCURRICULAR-PERIODO-ERROR-ACTUALIZAR";
    public const string DISENOCURRICULAR_PERIODO_ERROR_ELIMINAR = "DISENOCURRICULAR-PERIODO-ERROR-ELIMINAR";



    #endregion


}

