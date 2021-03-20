﻿using System.Text;
using Repository.Model;

namespace Generator
{
    internal static class FieldExtension
    {
        public static string WriteNative(this Field field, Namespace currentNamespace)
        {
            string type = field switch
            {
                {Callback: {} c} => c.SymbolName,
                _ => field.WriteType(Target.Native, currentNamespace)
            };

            var builder = new StringBuilder();
            builder.Append(field.WriteNativeSummary());

            if (type == "string")
                builder.AppendLine($"[MarshalAs(UnmanagedType.LPStr)]");

            var accessibility = field.Private ? "internal" : "public";
            
            builder.AppendLine($"{accessibility} {type} {field.SymbolName};");
            return builder.ToString();
        }
    }
}
