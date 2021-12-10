﻿namespace GirLoader.Output
{
    public partial class Constant : GirModel.Constant
    {
        GirModel.Namespace GirModel.Constant.Namespace => _repository.Namespace;
        string GirModel.Constant.Name => OriginalName;
        string GirModel.Constant.Value => Value;
        GirModel.Type GirModel.Constant.Type => TypeReference.GetResolvedType();
    }
}
