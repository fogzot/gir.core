﻿using System;
using System.Collections.Generic;
using Generator.Model;

namespace Generator.Renderer.Public.ParameterToNativeExpressions;

internal class Record : ToNativeParameterConverter
{
    public bool Supports(GirModel.AnyType type)
        => type.Is<GirModel.Record>();

    public void Initialize(ParameterToNativeData parameter, IEnumerable<ParameterToNativeData> _)
    {
        if (parameter.Parameter.Direction != GirModel.Direction.In)
            throw new NotImplementedException($"{parameter.Parameter.AnyTypeOrVarArgs}: record parameter with direction != in not yet supported");

        if (!parameter.Parameter.IsPointer)
            throw new NotImplementedException($"{parameter.Parameter.AnyTypeOrVarArgs}: Not pointed record types can not yet be converted to native");

        var parameterName = Model.Parameter.GetName(parameter.Parameter);
        var variableName = parameter.Parameter.Nullable
            ? parameterName + "?.Handle ?? " + Model.Record.GetFullyQualifiedInternalNullHandleInstance((GirModel.Record) parameter.Parameter.AnyTypeOrVarArgs.AsT0.AsT0)
            : parameterName + ".Handle";

        parameter.SetSignatureName(parameterName);
        parameter.SetCallName(variableName);
    }
}
