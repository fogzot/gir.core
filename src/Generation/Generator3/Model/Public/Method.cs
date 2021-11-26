using System.Collections.Generic;
using System.Linq;

namespace Generator3.Model.Public
{
    public class Method
    {
        private readonly GirModel.Method _method;

        public string Name => _method.Name;
        public ReturnType ReturnType { get; }
        
        public InstanceParameter InstanceParameter { get; }
        public IEnumerable<Parameter> Parameters { get; }

        public Method(GirModel.Method method)
        {
            _method = method;

            ReturnType = method.ReturnType.CreatePublicModel();
            InstanceParameter = method.InstanceParameter.CreatePublicModel();
            Parameters = method.Parameters.CreatePublicModels();
        }

        public bool IsFree() => _method.IsFree() || _method.IsUnref();
        public bool HasInOutRefParameter() => _method.Parameters.Any(param => param.Direction != GirModel.Direction.In);
        public bool HasCallbackReturnType() => _method.ReturnType.AnyType.TryPickT0(out var type, out _) && type is GirModel.Callback;
        public bool HasUnionParameter() => _method.Parameters.Any(param => param.AnyTypeReference.AnyType.TryPickT0(out var type, out _) && type is GirModel.Union);
        public bool HasUnionReturnType() => _method.ReturnType.AnyType.TryPickT0(out var type, out _) && type is GirModel.Union;

        public bool HasArrayClassParameter() => _method.Parameters.Any(param => param.AnyTypeReference.AnyType.TryPickT1(out var arrayType, out _)
                                                                                && arrayType.AnyTypeReference.AnyType.TryPickT0(out var type, out _)
                                                                                && type is GirModel.Class);

        public bool HasArrayClassReturnType() => _method.ReturnType.AnyType.TryPickT1(out var arrayType, out _)
                                                 && arrayType.AnyTypeReference.AnyType.TryPickT0(out var type, out _)
                                                 && type is GirModel.Class;
    }
}
