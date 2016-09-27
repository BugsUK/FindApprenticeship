namespace SFA.Apprenticeships.Web.Common.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using CuttingEdge.Conditions;
    using Validators;
    using Validators.Extensions;
    using HtmlHelper = System.Web.Mvc.HtmlHelper;

    public static class HtmlExtensions
    {
        #region Text and TextArea

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormTextFor<TModel, TProperty>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null,
                    bool verified = false)
        {
            return BuildFormControl(helper, 
                                    expression, 
                                    helper.TextBoxFor, 
                                    labelText, 
                                    hintText, 
                                    false, 
                                    containerHtmlAttributes, 
                                    labelHtmlAttributes,
                                    hintHtmlAttributes,
                                    controlHtmlAttributes,
                                    verified);
        }

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormPasswordFor<TModel, TProperty>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper,
                                    expression,
                                    helper.PasswordFor,
                                    labelText,
                                    hintText,
                                    false,
                                    containerHtmlAttributes,
                                    labelHtmlAttributes,
                                    hintHtmlAttributes,
                                    controlHtmlAttributes);
        }

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormTextAreaFor<TModel, TProperty>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null,
                    string hintText = null,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            return BuildFormControl(helper, 
                                    expression, 
                                    helper.TextAreaFor, 
                                    labelText, 
                                    hintText, 
                                    true, 
                                    containerHtmlAttributes, 
                                    labelHtmlAttributes,
                                    hintHtmlAttributes,
                                    controlHtmlAttributes);
        }

        private static MvcHtmlString BuildFormControl<TModel, TProperty>(
                    HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> expression,
                    Func<Expression<Func<TModel, TProperty>>, IDictionary<string, object>, MvcHtmlString> controlFunc,
                    string labelText = null,
                    string hintText = null,
                    bool addMaxLengthCounter = false,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object hintHtmlAttributes = null,
                    object controlHtmlAttributes = null,
                    bool verified = false)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var validationType = GetValidationType(helper, expression);
            var containerAttributes = MergeAttributes("form-group", containerHtmlAttributes);
            var controlAttributes = MergeAttributes("form-control", controlHtmlAttributes);
            var labelAttributes = MergeAttributes("form-label", labelHtmlAttributes);
            var hintAttributes = MergeAttributes("form-hint", hintHtmlAttributes);

            var validator = helper.ValidationMessageWithSeverityFor(expression, null, validationType);

            var fieldContent = verified ? VerifiedControl(controlFunc, expression, controlAttributes) : controlFunc(expression, controlAttributes);

            return FormText(
                helper.LabelFor(expression, labelText, labelAttributes),
                helper.HintFor(expression, hintText, hintAttributes),
                fieldContent,
                validator,
                AnchorFor(helper, expression),
                addMaxLengthCounter ? CharactersLeftFor(helper, expression) : null,
                addMaxLengthCounter ? ScreenReaderSpan(helper, expression) : null,
                containerAttributes,
                validationType
                );
        }

        private static MvcHtmlString FormText(MvcHtmlString labelContent,
                                    MvcHtmlString hintContent,
                                    MvcHtmlString fieldContent,
                                    MvcHtmlString validationMessage,
                                    MvcHtmlString anchorTag,
                                    MvcHtmlString maxLengthSpan,
                                    MvcHtmlString ariaLimitVisuallyHidden,
                                    RouteValueDictionary containerHtmlAttributes,
                                    ValidationType validationType
                                    )
        {
            var container = new TagBuilder("div");
            container.MergeAttributes(containerHtmlAttributes);

            AddValidationCssClass(validationType, container);

            container.InnerHtml += string.Concat(anchorTag, labelContent, hintContent, fieldContent, maxLengthSpan, ariaLimitVisuallyHidden, validationMessage);

            return MvcHtmlString.Create(container.ToString());
        }

        private static void AddValidationCssClass(ValidationType validationType, TagBuilder container)
        {
            if (validationType == ValidationType.Error)
            {
                container.AddCssClass(GetValidationCssClass(validationType));
            }
            if (validationType == ValidationType.Warning)
            {
                container.AddCssClass(Validators.HtmlHelper.ValidationWarningInputCssClassName);
            }
        }

        public static string GetValidationCssClass(ValidationType validationType)
        {
            if (validationType == ValidationType.Error)
            {
                return HtmlHelper.ValidationInputCssClassName;
            }
            if (validationType == ValidationType.Warning)
            {
                return Validators.HtmlHelper.ValidationWarningInputCssClassName;
            }

            return "";
        }

        public static string HtmlRawEscaped(IHtmlString originalString)
        {
            return originalString.ToString().Replace("&amp;", "\u0026").Replace("&#39;", "\u0027").Replace("&nbsp;", "\u00A0");
        }

        public static string HtmlRawEscaped(string originalString)
        {
            return originalString.Replace("&amp;", "\u0026").Replace("&#39;", "\u0027").Replace("&nbsp;", "\u00A0");
        }

        #endregion

        #region Checkbox

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormCheckBoxFor<TModel>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, bool>> expression,
                    string labelText = null,
                    object containerHtmlAttributes = null,
                    object labelHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            var container = new TagBuilder("div");

            var containerAttributes = MergeAttributes("form-group", containerHtmlAttributes);
            
            var validationType = GetValidationType(helper, expression);
            var validator = helper.ValidationMessageWithSeverityFor(expression, null, validationType);
            var anchorTag = AnchorFor(helper, expression);

            container.MergeAttributes(containerAttributes);

            AddValidationCssClass(validationType, container);

            var label = GetLabel(helper, expression, labelText, labelHtmlAttributes, controlHtmlAttributes);

            container.InnerHtml += string.Concat(anchorTag, label.ToString(), validator);

            return MvcHtmlString.Create(container.ToString());
        }

        private static TagBuilder GetLabel<TModel>(HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression, string labelText, object labelHtmlAttributes, object controlHtmlAttributes)
        {
            var controlAttributes = MergeAttributes("", controlHtmlAttributes);
            var labelAttributes = MergeAttributes("", labelHtmlAttributes);
            var label = new TagBuilder("label");
            label.MergeAttributes(labelAttributes);
            label.Attributes.Add("for", helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
            label.InnerHtml = helper.CheckBoxFor(expression, controlAttributes).ToString();
            label.InnerHtml += GetDisplayName(helper, expression, labelText);
            return label;
        }

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormUnvalidatedCheckBoxFor<TModel>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, bool>> expression,
                    string labelText = null,
                    object labelHtmlAttributes = null,
                    object controlHtmlAttributes = null)
        {
            var label = GetLabel(helper, expression, labelText, labelHtmlAttributes, controlHtmlAttributes);

            return MvcHtmlString.Create(label.ToString());
        }

        #endregion

        #region Radio Buttons

        /// <summary>
        /// Creates the NAS form element with the appropriate classes.
        /// </summary>
        /// <returns>The html to render</returns>
        public static MvcHtmlString FormRadioButtonsYesNo<TModel>(
                    this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, bool>> expression,
                    string labelText = null,
                    object containerHtmlAttributes = null,
                    object subContainerHtmlAttributes = null,
                    object labelHtmlAttributes = null)
        {
            var container = new TagBuilder("div");
            var subContainer = new TagBuilder("div");
            var titleLabel = new TagBuilder("p");
            var yesLabel = new TagBuilder("label");
            var noLabel = new TagBuilder("label");

            var value = bool.Parse(helper.ValueFor(expression).ToString());

            var containerAttributes = MergeAttributes("form-group", containerHtmlAttributes);
            var subContainerAttributes = MergeAttributes("form-group form-group-compound", subContainerHtmlAttributes);
            var labelAttributes = MergeAttributes("form-label", labelHtmlAttributes);
            
            var yesLabelAttributes = MergeAttributes("block-label" + (value ? " selected" : ""), labelHtmlAttributes);
            var noLabelAttributes = MergeAttributes("block-label" + (!value ? " selected" : ""), labelHtmlAttributes);

            container.MergeAttributes(containerAttributes);
            subContainer.MergeAttributes(subContainerAttributes);
            titleLabel.MergeAttributes(labelAttributes);

            titleLabel.InnerHtml = GetDisplayName(helper, expression, labelText).ToString();

            var fieldId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            var yesRadio = helper.RadioButtonFor(expression, true, GetCheckedAttribute(fieldId + "-yes", value, true)) + "Yes";
            var noRadio = helper.RadioButtonFor(expression, false, GetCheckedAttribute(fieldId + "-no", value, false)) + "No";

            yesLabel.MergeAttributes(yesLabelAttributes);
            noLabel.MergeAttributes(noLabelAttributes);

            yesLabel.InnerHtml += yesRadio;
            noLabel.InnerHtml += noRadio;

            subContainer.InnerHtml += string.Format("{0}{1}{2}", titleLabel, yesLabel, noLabel);
            container.InnerHtml += subContainer.ToString();

            return MvcHtmlString.Create(container.ToString());
        }

        private static object GetCheckedAttribute(string id, bool value, bool field)
        {
            if (value == field)
            {
                return new {id, @checked = "checked"};
            }
            return new {id};
        }

        #endregion

        #region Hint, Anchor, Characters Left

        public static MvcHtmlString GetDisplayName<TModel, TProperty>(
                    this HtmlHelper<TModel> htmlHelper,
                    Expression<Func<TModel, TProperty>> expression,
                    string labelText = null)
        {
            if (!string.IsNullOrWhiteSpace(labelText))
            {
                return MvcHtmlString.Create(labelText);
            }

            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = metaData.DisplayName ?? (metaData.PropertyName ?? ExpressionHelper.GetExpressionText(expression));
            return MvcHtmlString.Create(value);
        }

        private static MvcHtmlString HintFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string hintText, IDictionary<string, object> htmlAttributes)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var labelText = hintText ?? metadata.Description ?? string.Empty;
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("span");
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            tag.SetInnerText(labelText);
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        private static MvcHtmlString AnchorFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var elementId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(metadata.PropertyName);
            
            if (string.IsNullOrWhiteSpace(elementId))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("a");
            tag.Attributes.Add("name", elementId.ToLower());
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        private static MvcHtmlString CharactersLeftFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();
            
            var tagHint = new TagBuilder("span");
            var tagCount = new TagBuilder("span");
            var tagText = new TagBuilder("span");

            tagHint.Attributes.Add("class", "form-hint hide-nojs");
            tagCount.Attributes.Add("class", "maxchar-count");
            tagText.Attributes.Add("class", "maxchar-text");

            tagCount.SetInnerText("");
            tagText.SetInnerText(" characters remaining");
            tagHint.InnerHtml = tagCount + "\r\n" + tagText;

            return MvcHtmlString.Create(tagHint.ToString(TagRenderMode.Normal));
        }


        private static MvcHtmlString ScreenReaderSpan<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            Condition.Requires(helper, "helper").IsNotNull();
            Condition.Requires(expression, "expression").IsNotNull();

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var tag = new TagBuilder("span");
            tag.Attributes.Add("aria-live", "polite");
            tag.Attributes.Add("class", "visuallyhidden aria-limit");

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
        private static MvcHtmlString VerifiedControl<TModel, TProperty>(Func<Expression<Func<TModel, TProperty>>, IDictionary<string, object>, MvcHtmlString> controlFunc, Expression<Func<TModel, TProperty>> expression, RouteValueDictionary controlAttributes)
        {
            var div = new TagBuilder("div");
            div.Attributes.Add("class", "input-withlink");

            var control = controlFunc(expression, controlAttributes);

            var span = new TagBuilder("span");
            span.Attributes.Add("id", "verifyContainer");
            span.Attributes.Add("class", "input-withlink__link");

            var innerSpan = new TagBuilder("span");

            var i = new TagBuilder("i");
            i.Attributes.Add("class", "fa fa-check-circle-o");

            innerSpan.InnerHtml = i + "Verified";

            span.InnerHtml = innerSpan.ToString();

            div.InnerHtml = control + "\r\n" + span;

            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region Helpers

        public static ValidationType GetValidationType<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var htmlFieldPrefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            var fullyQualifiedName = string.IsNullOrEmpty(htmlFieldPrefix) ? expressionText : string.Join(".", htmlFieldPrefix, expressionText);
            if (!helper.ViewData.ModelState.IsValidField(fullyQualifiedName) && helper.ViewData.ModelState.ContainsKey(fullyQualifiedName))
            {
                if (helper.ViewData.ModelState[fullyQualifiedName].Errors.Any(e => e.GetType() == typeof(ModelError)))
                {
                    return ValidationType.Error;
                }
                if (helper.ViewData.ModelState[fullyQualifiedName].Errors.Any(e => e.GetType() == typeof(ModelWarning)))
                {
                    return ValidationType.Warning;
                }
            }
            return ValidationType.None;
        }

        private static RouteValueDictionary MergeAttributes(string baseClassName, object extendedAttributes)
        {
            var mergeAttributes = extendedAttributes != null ? extendedAttributes as RouteValueDictionary ?? HtmlHelper.AnonymousObjectToHtmlAttributes(extendedAttributes) : new RouteValueDictionary();

            if (mergeAttributes.ContainsKey("baseClassName"))
            {
                baseClassName = mergeAttributes["baseClassName"].ToString();
                mergeAttributes.Remove("baseClassName");
            }

            if (mergeAttributes.ContainsKey("class"))
            {
                mergeAttributes["class"] += " " + baseClassName;
            }
            else
            {
                mergeAttributes.Add("class", baseClassName);
            }

            return mergeAttributes;
        }

        #endregion
    }
}