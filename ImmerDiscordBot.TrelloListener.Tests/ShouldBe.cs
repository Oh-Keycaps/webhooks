using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework.Constraints;

namespace ImmerDiscordBot.TrelloListener
{
    public static class ShouldBe
    {
        public static Constraint StructEquivalentTo(object expected)
        {
            var constraints = expected.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x => new StructFieldConstraint(x.Name, x.GetValue(expected)));
            return new ObjectPropertyCollectionConstraint(constraints);
        }
        public static Constraint EquivalentTo(object expected)
        {
            var constraints = expected.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x => new ObjectPropertiesConstraint(x.Name, x.GetValue(expected)));
            return new ObjectPropertyCollectionConstraint(constraints);
        }
    }

    internal class ObjectPropertyCollectionConstraint : Constraint
    {
        private readonly Constraint[] _constraints;

        public ObjectPropertyCollectionConstraint(IEnumerable<Constraint> constraints)
        {
            _constraints = constraints.ToArray();
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var name = actual.GetType().FullName;
            var description = new StringBuilder($"{name} should have the following properties and values:").AppendLine();
            var failure = new StringBuilder("The following constraints failed:").AppendLine();
            var results = _constraints.Select(c =>
            {
                try
                {
                    return c.ApplyTo(actual);
                }
                catch (Exception e)
                {
                    return new ConstraintResult(c, e, ConstraintStatus.Error);
                }
            }).ToList();
            var constraintStatus = results.All(x => x.IsSuccess) ?
                    ConstraintStatus.Success :
                results.Any(x => x.Status == ConstraintStatus.Error) ?
                    ConstraintStatus.Error : ConstraintStatus.Failure;
            if (constraintStatus != ConstraintStatus.Success)
            {
                foreach (var result in results)
                {
                    description.AppendLine(result.Description);
                    if (result.IsSuccess) continue;
                    switch (result.Name)
                    {
                        case "PropertyExists":
                            failure.AppendFormat("{0} does not exist on object {1}", result.Description, name).AppendLine();
                            break;
                        case "Property":
                            failure.AppendFormat("{0} but was: {1}", result.Description, result.ActualValue).AppendLine();
                            break;
                        default:
                            failure.AppendFormat("Test {1}: {0} - {2}. Actual Value: {3}", result.Description, result.Name, result.Status, result.ActualValue).AppendLine();
                            break;
                    }
                }
            }

            Description = description.ToString();
            return new ConstraintResult(this, failure, constraintStatus);
        }
    }
    internal class ObjectPropertiesConstraint : Constraint
    {
        private readonly PropertyExistsConstraint _propertyConstraint;
        private readonly PropertyConstraint _equalConstraint;

        public ObjectPropertiesConstraint(string name, object value)
        {
            _equalConstraint = new PropertyConstraint(name, new EqualConstraint(value));
            _propertyConstraint = new PropertyExistsConstraint(name);
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Description = _equalConstraint.Description;
            var result = _propertyConstraint.ApplyTo(actual);
            if (result.IsSuccess)
            {
                var constraintResult = _equalConstraint.ApplyTo(actual);
                return constraintResult;
            }

            return result;
        }
    }
    internal class StructFieldConstraint : Constraint
    {
        private readonly FieldExistsConstraint _propertyConstraint;
        private readonly FieldConstraint _equalConstraint;

        public StructFieldConstraint(string name, object value)
        {
            _equalConstraint = new FieldConstraint(name, new EqualConstraint(value));
            _propertyConstraint = new FieldExistsConstraint(name);
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Description = _equalConstraint.Description;
            var result = _propertyConstraint.ApplyTo(actual);
            if (result.IsSuccess)
            {
                var constraintResult = _equalConstraint.ApplyTo(actual);
                return constraintResult;
            }

            return result;
        }
    }
    internal class FieldConstraint : Constraint
    {
        private readonly PropertyExistsConstraint _propertyConstraint;
        private readonly PropertyConstraint _equalConstraint;

        public FieldConstraint(string name, object value)
        {
            _equalConstraint = new PropertyConstraint(name, new EqualConstraint(value));
            _propertyConstraint = new PropertyExistsConstraint(name);
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Description = _equalConstraint.Description;
            var result = _propertyConstraint.ApplyTo(actual);
            if (result.IsSuccess)
            {
                var constraintResult = _equalConstraint.ApplyTo(actual);
                return constraintResult;
            }

            return result;
        }
    }
    internal class FieldExistsConstraint : Constraint
    {
        private readonly string _name;

        public FieldExistsConstraint(string name)
        {
            _name = name;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var fields = actual.GetType().GetFields();
            var fieldType = actual.GetType().GetField(_name, BindingFlags.Instance | BindingFlags.Public);
            return new ConstraintResult(this, actual, fieldType != null);
        }
    }
}
